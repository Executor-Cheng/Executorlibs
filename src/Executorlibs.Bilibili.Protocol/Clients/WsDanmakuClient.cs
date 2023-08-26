using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Dispatchers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Options;
using Executorlibs.Bilibili.Protocol.Services;
using Executorlibs.Shared.Extensions;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public abstract class WsDanmakuClient<TDecoder> : DanmakuClient<WebSocketConnectionContext, TDecoder> where TDecoder : PayloadDecoder, new()
    {
        protected readonly IDanmakuServerProvider _credentialProvider;

        protected WsDanmakuClient(IBilibiliRawdataDispatcher invoker, IDanmakuServerProvider credentialProvider, IBilibiliMessageDispatcher<IDisconnectedMessage>? disconnectDispatcher = null) : base(invoker, disconnectDispatcher)
        {
            _credentialProvider = credentialProvider;
        }

        protected override async Task ConnectAsync(WebSocketConnectionContext context, CancellationToken connectToken)
        {
            var userCredentials = await _credentialProvider.GetDanmakuServerInfoAsync(context.RoomId, connectToken);
            var webSocket = context.WebSocket;
            var serverHost = userCredentials.Hosts[(int)(Stopwatch.GetTimestamp() % userCredentials.Hosts.Length)];
            await webSocket.ConnectAsync(new Uri($"wss://{serverHost.Host}:{serverHost.WssPort}/sub"), connectToken);
            await SendJoinRoomAsync(webSocket, context.RoomId, userCredentials.UserId, userCredentials.Token, connectToken);
            await ValidateJoinRoomResultAsync(context, connectToken);
            var token = context.ConnectionCts.Token;
            _ = SendHeartBeatAsyncLoop(context, token);
            _ = ReceiveMessageAsyncLoop(context, token);
        }

        protected override ValueTask SendAsync(ReadOnlyMemory<byte> memory, CancellationToken token)
        {
            var context = GetContext();
            return context.WebSocket.SendAsync(memory, WebSocketMessageType.Binary, true, token);
        }

        protected override ValueTask ReceiveAsync(WebSocketConnectionContext context, Memory<byte> memory, CancellationToken token)
        {
            return context.WebSocket.ReceiveFullyAsync(memory, token);
        }

        protected abstract ValueTask SendJoinRoomAsync(WebSocket client, uint roomId, ulong userId, string token, CancellationToken cToken = default);

        protected override WebSocketConnectionContext CreateContext(DanmakuClientOptions options)
        {
            return new WebSocketConnectionContext(options);
        }
    }
}
