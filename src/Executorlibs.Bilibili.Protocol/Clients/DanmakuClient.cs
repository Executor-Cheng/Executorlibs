using System;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Dispatchers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Options;
using Executorlibs.Bilibili.Protocol.Utility;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.Shared.Exceptions;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public interface IDanmakuClient : IMessageClient
    {
        uint RoomId { get; }

        bool Connected { get; }

        Task ConnectAsync(DanmakuClientOptions options, CancellationToken token = default);

        void Disconnect();
    }

    public abstract class DanmakuClient : IDanmakuClient
    {
        protected static readonly ReadOnlyMemory<byte> HeartBeatPacket = new byte[16] { 0, 0, 0, 16, 0, 16, 0, 2, 0, 0, 0, 2, 0, 0, 0, 1 };

        protected static byte[] CreatePayload(uint action)
        {
            byte[] buffer = new byte[16];
            ref DanmakuProtocol protocol = ref DanmakuProtocolUtility.AsProtocol(buffer);
            protocol.PacketLength = (uint)buffer.Length;
            protocol.Action = action;
            protocol.HeaderLength = 16;
            protocol.Parameter = 1;
            protocol.Version = 2;
            protocol.ChangeEndian();
            return buffer;
        }

        protected static byte[] CreatePayload(uint action, string body)
        {
            var buffer = new byte[16 + Encoding.UTF8.GetByteCount(body)];
            var span = buffer.AsSpan();
            ref DanmakuProtocol protocol = ref DanmakuProtocolUtility.AsProtocol(span);
            protocol.PacketLength = (uint)buffer.Length;
            protocol.Action = action;
            protocol.HeaderLength = 16;
            protocol.Parameter = 1;
            protocol.Version = 2;
            protocol.ChangeEndian();
#if NETSTANDARD2_0
            Encoding.UTF8.GetBytes(body, 0, body.Length, buffer, 16);
#else
            Encoding.UTF8.GetBytes(body, span[16..]);
#endif
            return buffer;
        }

        protected static byte[] CreatePayload(uint action, byte[] body)
        {
            byte[] buffer = new byte[16 + body.Length];
            var span = buffer.AsSpan();
            ref DanmakuProtocol protocol = ref DanmakuProtocolUtility.AsProtocol(span);
            protocol.PacketLength = (uint)buffer.Length;
            protocol.Action = action;
            protocol.HeaderLength = 16;
            protocol.Parameter = 1;
            protocol.Version = 2;
            protocol.ChangeEndian();
#if NET5_0_OR_GREATER
            Unsafe.CopyBlock(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), 16), ref MemoryMarshal.GetArrayDataReference(body), (uint)body.Length);
#else
            Unsafe.CopyBlock(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), 16), ref MemoryMarshal.GetReference(body.AsSpan()), (uint)body.Length);
#endif
            return buffer;
        }

        protected static byte[] CreateJoinRoomPayload(byte version, uint roomId, ulong userId, string token)
        {
            byte[] json = JsonSerializer.SerializeToUtf8Bytes(new
            {
                uid = userId,
                roomid = roomId,
                protover = version,
                //platform = "web",
                //clientver = "1.13.4",
                //type = 2,
                key = token
            });
            return CreatePayload(7, json);
        }

        public abstract bool Connected { get; }

        public abstract uint RoomId { get; }

        protected volatile uint _disposed;

        protected IBilibiliRawdataDispatcher _invoker;

        protected DanmakuClient(IBilibiliRawdataDispatcher invoker)
        {
            _invoker = invoker;
        }

        protected void CheckDisposed()
        {
            if (_disposed != 0)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        public abstract Task ConnectAsync(DanmakuClientOptions options, CancellationToken token = default);

        public abstract void Disconnect();

        protected abstract ValueTask SendAsync(ReadOnlyMemory<byte> memory, CancellationToken token);
    }

    public abstract class DanmakuClient<TContext> : DanmakuClient where TContext : ConnectionContext
    {
        public override bool Connected => _context?.Connected ?? false;

        public override uint RoomId => _context?.RoomId ?? 0;

        protected readonly IBilibiliMessageDispatcher<IDisconnectedMessage>? _disconnectDispatcher;

        protected TContext? _context;

        protected DanmakuClient(IBilibiliRawdataDispatcher invoker,
                                IBilibiliMessageDispatcher<IDisconnectedMessage>? disconnectDispatcher = null) : base(invoker)
        {
            _disconnectDispatcher = disconnectDispatcher;
        }

        protected abstract TContext CreateContext(DanmakuClientOptions options);

        protected TContext GetContext()
        {
            var context = _context;
            return context ?? throw new OperationCanceledException();
        }

        protected abstract ValueTask ReceiveAsync(TContext context, Memory<byte> memory, CancellationToken token);

        public override async Task ConnectAsync(DanmakuClientOptions options, CancellationToken token = default)
        {
            CheckDisposed();
            var previousCtx = Volatile.Read(ref _context);
            if (previousCtx == null)
            {
                var createdCtx = CreateContext(options);
                var connectionToken = createdCtx.ConnectionCts!.Token;
                if (Interlocked.CompareExchange(ref _context, createdCtx, null) == null)
                {
                    var connectCts = CancellationTokenSource.CreateLinkedTokenSource(connectionToken, token);
                    try
                    {
                        CheckDisposed();
                        var connectToken = connectCts.Token;
                        await ConnectAsync(createdCtx, connectToken);
                        createdCtx.ClientConnected();
                        return;
                    }
                    catch (Exception)
                    {
                        if (Interlocked.CompareExchange(ref _context, null, createdCtx) == createdCtx)
                        {
                            createdCtx.Dispose();
                        }
                        connectionToken.ThrowIfCancellationRequested();
                        throw;
                    }
                    finally
                    {
                        connectCts.Dispose();
                    }
                }
                createdCtx.Dispose();
            }
            throw new InvalidOperationException();
        }

        protected abstract Task ConnectAsync(TContext context, CancellationToken connectToken);

        public override void Disconnect()
        {
            var context = Volatile.Read(ref _context);
            if (context != null)
            {
                Disconnect(context, null);
            }
        }

        protected virtual void Disconnect(TContext context, Exception? exception)
        {
            if (Interlocked.CompareExchange(ref _context, null, context) == context)
            {
                context.Dispose();
                if (_disconnectDispatcher is IBilibiliMessageDispatcher<IDisconnectedMessage> disconnectDispatcher)
                {
                    _ = disconnectDispatcher.HandleMessageAsync(this, new DisconnectedMessage(new DanmakuClientOptions(context.RoomId, context.HeartbeatInterval), exception));
                }
            }
        }
        
        protected virtual async Task SendHeartBeatAsyncLoop(TContext context, CancellationToken token)
        {
            double tickFrequency = 10000 * 1000 / (double)Stopwatch.Frequency;
            long ticks;
            while (true)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    ticks = Stopwatch.GetTimestamp();
                    await SendAsync(HeartBeatPacket, token).ConfigureAwait(false);
                    var toSleep = context.HeartbeatInterval - TimeSpan.FromTicks((long)((Stopwatch.GetTimestamp() - ticks) * tickFrequency));
                    if (toSleep <= default(TimeSpan))
                    {
                        throw new TimeoutException("Heartbeat timed out.");
                    }
                    await Task.Delay(toSleep, token);
                }
                catch (OperationCanceledException) when (token.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception e)
                {
                    Disconnect(context, e);
                    return;
                }
            }
        }

        protected virtual async Task ReceiveMessageAsyncLoop(TContext context, CancellationToken token)
        {
            var recvBuffer = new byte[4096];
            var client = (IDanmakuClient)new DanmakuClientContext(this, context.RoomId); // 预先提升到堆
            while (true)
            {
                try
                {
                    await ReceiveAsync(context, recvBuffer.AsMemory(0, 16), token).ConfigureAwait(false);
                    uint packetLength = HandleEndianessAndGetPacketLength(recvBuffer);
                    uint payloadLength = packetLength - DanmakuProtocol.Size;
                    if (payloadLength != 0)
                    {
                        if (packetLength > 65535)
                        {
                            throw new InvalidDataException($"包长度过大:{packetLength}");
                        }
                        if (packetLength > recvBuffer.Length)
                        {
                            recvBuffer = new byte[packetLength];
                        }
                        await ReceiveAsync(context, new Memory<byte>(recvBuffer, (int)DanmakuProtocol.Size, (int)payloadLength), token);
                    }
                    try
                    {
                        await _invoker.HandleRawdataAsync(client, recvBuffer);
                    }
                    catch (Exception)
                    {

                    }
                }
                catch (OperationCanceledException) when (token.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception e)
                {
                    Disconnect(context, e);
                    return;
                }
            }
        }

        protected virtual async Task ValidateJoinRoomResultAsync(TContext context, CancellationToken token)
        {
            byte[] recvBuffer = new byte[26];
            await ReceiveOnceAsync(context, recvBuffer, token);
            if (DanmakuProtocolUtility.AsProtocol(recvBuffer).Action == 8)
            {
                uint payloadLength = DanmakuProtocolUtility.AsProtocol(recvBuffer).PacketLength - DanmakuProtocol.Size;
                using var j = JsonDocument.Parse(recvBuffer.AsMemory(16, (int)payloadLength));
                var root = j.RootElement;
                if (root.GetProperty("code").GetInt32() == 0)
                {
                    return;
                }
                throw new UnknownResponseException(root);
            }
            throw new UnknownResponseException();
        }

        protected static uint HandleEndianessAndGetPacketLength(byte[] recvBuffer)
        {
            ref var protocol = ref DanmakuProtocolUtility.AsProtocol(recvBuffer);
            protocol.ChangeEndian();
            return protocol.PacketLength;
        }

        protected async Task ReceiveOnceAsync(TContext context, byte[] recvBuffer, CancellationToken token)
        {
            await ReceiveAsync(context, recvBuffer.AsMemory(0, 16), token).ConfigureAwait(false);
            uint packetLength = HandleEndianessAndGetPacketLength(recvBuffer);
            uint payloadLength = packetLength - DanmakuProtocol.Size;
            if (payloadLength != 0)
            {
                if (packetLength > 65535)
                {
                    throw new InvalidDataException($"包长度过大:{packetLength}");
                }
                if (packetLength > recvBuffer.Length)
                {
                    recvBuffer = new byte[packetLength];
                }
                await ReceiveAsync(context, new Memory<byte>(recvBuffer, (int)DanmakuProtocol.Size, (int)payloadLength), token);
            }
        }
    }

    public abstract class DanmakuClient<TContext, TDecoder> : DanmakuClient<TContext> where TContext : ConnectionContext
                                                                                      where TDecoder : PayloadDecoder, new()
    {
        protected DanmakuClient(IBilibiliRawdataDispatcher invoker,
                                IBilibiliMessageDispatcher<IDisconnectedMessage>? disconnectDispatcher = null) : base(invoker, disconnectDispatcher)
        {

        }

        protected override async Task ReceiveMessageAsyncLoop(TContext context, CancellationToken token)
        {
            var recvBuffer = new byte[4096];
            var client = (IDanmakuClient)new DanmakuClientContext(this, context.RoomId); // 预先提升到堆
            var decoder = new TDecoder();
            while (true)
            {
                try
                {
                    await ReceiveAsync(context, recvBuffer.AsMemory(0, 16), token).ConfigureAwait(false);
                    uint packetLength = HandleEndianessAndGetPacketLength(recvBuffer);
                    uint payloadLength = packetLength - DanmakuProtocol.Size;
                    if (payloadLength != 0)
                    {
                        if (packetLength > 65535)
                        {
                            throw new InvalidDataException($"包长度过大:{packetLength}");
                        }
                        if (packetLength > recvBuffer.Length)
                        {
                            recvBuffer = new byte[packetLength];
                        }
                        await ReceiveAsync(context, new Memory<byte>(recvBuffer, (int)DanmakuProtocol.Size, (int)payloadLength), token);
                    }
                    if (decoder.TryOpen(recvBuffer))
                    {
                        try
                        {
                            while (decoder.TryProcess(out var decodedRawdata))
                            {
                                await _invoker.HandleRawdataAsync(client, decodedRawdata!);
                            }
                        }
                        finally
                        {
                            decoder.Close();
                        }
                    }
                    else
                    {
                        await _invoker.HandleRawdataAsync(client, recvBuffer);
                    }
                }
                catch (OperationCanceledException) when (token.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception e)
                {
                    Disconnect(context, e);
                    return;
                }
            }
        }
    }
}
