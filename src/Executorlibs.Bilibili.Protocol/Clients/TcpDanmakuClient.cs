using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Dispatchers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Options;
using Executorlibs.Bilibili.Protocol.Services;
using Executorlibs.Shared.Extensions;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public abstract class TcpDanmakuClient<TDecoder> : DanmakuClient<TcpConnectionContext, TDecoder> where TDecoder : PayloadDecoder, new()
    {
        protected readonly IDanmakuServerProvider _credentialProvider;

        protected TcpDanmakuClient(IBilibiliRawdataDispatcher invoker, IDanmakuServerProvider credentialProvider, IBilibiliMessageDispatcher<IDisconnectedMessage>? disconnectDispatcher = null) : base(invoker, disconnectDispatcher)
        {
            _credentialProvider = credentialProvider;
        }

        protected override async Task ConnectAsync(TcpConnectionContext context, CancellationToken connectToken)
        {
            var servers = await _credentialProvider.GetDanmakuServerInfoAsync(context.RoomId, connectToken);
            var socket = context.Socket;
            var serverHost = servers.Hosts[(int)(Stopwatch.GetTimestamp() % servers.Hosts.Length)];
#if NET5_0_OR_GREATER
            await socket.ConnectAsync(serverHost.Host, serverHost.Port, connectToken);
#else
            await socket.ConnectAsync(serverHost.Host, serverHost.Port);
#endif
            await SendJoinRoomAsync(socket, context.RoomId, servers.UserId, servers.Token, connectToken);
            await ValidateJoinRoomResultAsync(context, connectToken);
            var token = context.ConnectionCts.Token;
            _ = SendHeartBeatAsyncLoop(context, token);
            _ = ReceiveMessageAsyncLoop(context, token);
        }

        protected override ValueTask SendAsync(ReadOnlyMemory<byte> memory, CancellationToken token)
        {
            var context = GetContext();
            var task = context.Socket.SendAsync(memory, SocketFlags.None, token);
            if (task.IsCompletedSuccessfully)
            {
                task.GetAwaiter().GetResult();
                return default;
            }
            return new ValueTask(task.AsTask());
        }

        protected override ValueTask ReceiveAsync(TcpConnectionContext context, Memory<byte> memory, CancellationToken token)
        {
            return context.Socket.ReceiveFullyAsync(memory, SocketFlags.None, token);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract ValueTask SendJoinRoomAsync(Socket socket, uint roomId, ulong userId, string token, CancellationToken cToken = default);

        protected override TcpConnectionContext CreateContext(DanmakuClientOptions options)
        {
            return new TcpConnectionContext(options);
        }
    }
}
