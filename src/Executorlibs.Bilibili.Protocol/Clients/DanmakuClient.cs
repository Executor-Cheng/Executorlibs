using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients.Contexts;
using Executorlibs.Bilibili.Protocol.Clients.Coding;
using Executorlibs.Bilibili.Protocol.Options;
using Executorlibs.Bilibili.Protocol.Utility;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.Shared.Exceptions;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public interface IDanmakuClient : IMessageClient
    {
        uint RoomId { get; }

        ulong UserId { get; }

        bool Connected { get; }

        Task ConnectAsync(DanmakuClientOptions options, CancellationToken token = default);

        void Disconnect();
    }

    /*
    //public interface IDanmakuEventHandler<TContext> where TContext : ConnectionContext
    //{
    //    void HandleDisconnect(TContext context, Exception? exception);

    //    Task HandlePacketAsync(byte[] buffer, uint length);
    //}

    //public abstract class SimpleDanmakuEventHandler<TContext> : IDanmakuEventHandler<TContext> where TContext : ConnectionContext
    //{
    //    public event Action<TContext, Exception?>? OnDisconnect;

    //    public void HandleDisconnect(TContext context, Exception? exception)
    //    {
    //        OnDisconnect?.Invoke(context, exception);
    //    }

    //    public abstract Task HandlePacketAsync(byte[] buffer, uint length);
    //}

    //public class MessageDanmakuEventHandler<TContext> : IDanmakuEventHandler<TContext> where TContext : ConnectionContext
    //{
    //    protected readonly IBilibiliRawdataDispatcher _invoker;

    //    protected readonly IBilibiliMessageDispatcher<IDisconnectedMessage>? _disconnectDispatcher;

    //    public MessageDanmakuEventHandler(IBilibiliRawdataDispatcher invoker, IBilibiliMessageDispatcher<IDisconnectedMessage>? disconnectDispatcher = null)
    //    {
    //        _invoker = invoker;
    //        _disconnectDispatcher = disconnectDispatcher;
    //    }

    //    public void HandleDisconnect(TContext context, Exception? exception)
    //    {
    //        _disconnectDispatcher?.HandleMessageAsync(?, new DisconnectedMessage(context.Options, exception));
    //    }

    //    public Task HandlePacketAsync(byte[] buffer, uint length)
    //    {
    //        return _invoker.HandleRawdataAsync(?, buffer);
    //    }
    //}
    */

    public abstract class DanmakuClient<TEvent, TTransport> : DanmakuAdapter<TEvent, TTransport>, IDanmakuClient where TEvent : IDanmakuEventContext where TTransport : DanmakuTransport
    {
        public override bool Connected => _context?.Transport.Connected ?? false;

        public override uint RoomId => _context?.Options.RoomId ?? 0;

        public virtual ulong UserId => _context?.ServerInfo.UserId ?? 0;

        protected readonly DanmakuClientContextFactory<TTransport> _contextFactory;

        protected DanmakuClientContext<TTransport>? _context;

        /// <summary>
        /// 1: Avalible 2: Connecting 4: Disconning
        /// </summary>
        protected volatile int _flags;

        protected DanmakuClient(TEvent handler, DanmakuClientContextFactory<TTransport> contextFactory) : base(handler)
        {
            _contextFactory = contextFactory;
            _flags = 1;
        }

        public void Disconnect()
        {
            Disconnect(null);
        }

        protected void Disconnect(Exception? exception)
        {
            int flags = _flags;
            if ((flags & 0x5) != 1 || ((flags = Interlocked.CompareExchange(ref _flags, 3, 1)) & 0x5) != 1)
            {
                if ((flags & 1) == 0)
                {
                    throw new ObjectDisposedException(nameof(DanmakuClient<TEvent, TTransport>));
                }
                return;
            }
            var context = Interlocked.Exchange(ref _context, null);
            if (context != null)
            {
                try
                {
                    if (exception != null)
                    {
                        _event.HandleDisconnect(exception);
                    }
                }
                finally
                {
                    context.Dispose();
                }
            }
        }

        public async Task ConnectAsync(DanmakuClientOptions options, CancellationToken token = default)
        {
            int flags = Interlocked.CompareExchange(ref _flags, 3, 1);
            if (flags != 1)
            {
                if ((flags & 1) == 0)
                {
                    throw new ObjectDisposedException(nameof(DanmakuClient<TEvent, TTransport>));
                }
                throw new DuplicateOperationException();
            }
            // 0b011
            var context = await _contextFactory.CreateAsync(options, token);
            try
            {
                await SendJoinRoomAsync(context, token);
                await ValidateJoinRoomResultAsync(context.Transport, token);
                token = context.Transport.Token;
                _ = SendHeartBeatAsyncLoop(context.Transport, options.HeartbeatInterval, token);
                _ = ReceiveMessageAsyncLoop(context.Transport, options.HeartbeatInterval.Add(TimeSpan.FromSeconds(10)), token);
                _context = context;
                flags = Interlocked.CompareExchange(ref _flags, 1, 3);
                if (flags != 3)
                {

                }
                return;
            }
            catch
            {
                context.Dispose();
                throw;
            }
        }

        protected ValueTask SendJoinRoomAsync(DanmakuClientContext<TTransport> context, CancellationToken connectToken = default)
        {
            var options = context.Options;
            var serverInfo = context.ServerInfo;
            return context.Transport.SendAsync(DanmakuProtocolUtility.CreateJoinRoomPayload(2, options.RoomId, serverInfo.UserId, serverInfo.Buvid, options.Platform, serverInfo.Token), connectToken);
        }

        protected async Task SendHeartBeatAsyncLoop(TTransport transport, TimeSpan interval, CancellationToken token)
        {
            double tickFrequency = 10000 * 1000 / (double)Stopwatch.Frequency;
            long ticks;
            while (true)
            {
                token.ThrowIfCancellationRequested();
                ticks = Stopwatch.GetTimestamp();
                await transport.SendAsync(DanmakuProtocolUtility.HeartBeatPacket, token).ConfigureAwait(false);
                var toSleep = interval - TimeSpan.FromTicks((long)((Stopwatch.GetTimestamp() - ticks) * tickFrequency));
                if (toSleep <= default(TimeSpan))
                {
                    throw new TimeoutException("Heartbeat timed out.");
                }
                await Task.Delay(toSleep, token);
            }
        }

        protected async Task ValidateJoinRoomResultAsync(TTransport transport, CancellationToken token)
        {
            byte[] recvBuffer = new byte[26];
            await transport.ReceiveAsync(recvBuffer.AsMemory(0, 16), token).ConfigureAwait(false);
            uint packetLength = DanmakuProtocolUtility.HandleEndianessAndGetPacketLength(recvBuffer);
            if (DanmakuProtocolUtility.AsProtocol(recvBuffer).Action == 8)
            {
                uint payloadLength = packetLength - DanmakuProtocol.Size;
                if (payloadLength != 0)
                {
                    if (packetLength > 65535)
                    {
                        throw new InvalidDataException($"包长度过大:{packetLength}");
                    }
                    if (packetLength > recvBuffer.Length)
                    {
                        var buffer = new byte[packetLength];
                        DanmakuProtocolUtility.AsProtocol(buffer) = DanmakuProtocolUtility.AsProtocol(recvBuffer);
                        recvBuffer = buffer;
                    }
                    await transport.ReceiveAsync(new Memory<byte>(recvBuffer, (int)DanmakuProtocol.Size, (int)payloadLength), token);
                }
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
    }
}
