using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Clients.Contexts;
using Executorlibs.Bilibili.Protocol.Utility;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public abstract class DanmakuAdapter
    {
        public abstract bool Connected { get; }

        public abstract uint RoomId { get; }
    }

    public abstract class DanmakuAdapter<TEvent, TTransport> : DanmakuAdapter where TEvent : IDanmakuEventContext where TTransport : DanmakuTransport
    {
        protected readonly TEvent _event;

        protected DanmakuAdapter(TEvent handler)
        {
            _event = handler;
        }

        protected async Task ReceiveMessageAsyncLoop(TTransport transport, TimeSpan recvTimeout, CancellationToken token)
        {
            var recvBuffer = new byte[4096];
            var timeoutCts = CreateTimeoutCts(token);
            try
            {
                while (true)
                {
                    try
                    {
                        timeoutCts.CancelAfter(recvTimeout);
                        await transport.ReceiveAsync(recvBuffer.AsMemory(0, 16), timeoutCts.Token).ConfigureAwait(false);
                        uint packetLength = DanmakuProtocolUtility.HandleEndianessAndGetPacketLength(recvBuffer);
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
                            await transport.ReceiveAsync(new Memory<byte>(recvBuffer, (int)DanmakuProtocol.Size, (int)payloadLength), timeoutCts.Token);
                        }
                        timeoutCts.CancelAfter(Timeout.Infinite);
                        if (timeoutCts.IsCancellationRequested)
                        {
                            timeoutCts = CreateTimeoutCts(token);
                        }
                        await _event.HandlePacketAsync(recvBuffer, packetLength);
                    }
                    catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
                    {
                        throw new SocketException((int)SocketError.TimedOut);
                    }
                }
            }
            finally
            {
                timeoutCts.Dispose();
            }
        }

        protected static CancellationTokenSource CreateTimeoutCts(CancellationToken token)
        {
            return CancellationTokenSource.CreateLinkedTokenSource(token);
        }
    }
}
