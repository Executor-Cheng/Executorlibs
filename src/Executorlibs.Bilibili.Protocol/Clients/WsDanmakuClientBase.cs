using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Invokers;
using Executorlibs.Bilibili.Protocol.Models;
using Executorlibs.Bilibili.Protocol.Options;
using Executorlibs.Bilibili.Protocol.Services;
using Executorlibs.Shared.Extensions;
using Microsoft.Extensions.Options;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public abstract class WsDanmakuClientBase : DanmakuClientBase
    {
        protected volatile WebSocket? _Client;

        protected WsDanmakuClientBase(IBilibiliMessageHandlerInvoker invoker, IOptionsSnapshot<DanmakuClientOptions> options, DanmakuServerProvider credentialProvider) : base(invoker, options, credentialProvider)
        {

        }

        protected override async Task InternalConnectAsync(CancellationToken token)
        {
            int roomId = _Options.RoomId;
            DanmakuServerInfo server = await _CredentialProvider.GetDanmakuServerInfoAsync(token);
            ClientWebSocket client = new ClientWebSocket();
            client.Options.KeepAliveInterval = Timeout.InfiniteTimeSpan;
            token.Register(client.Dispose);
            DanmakuServerHostInfo serverHost = server.Hosts[(int)(Stopwatch.GetTimestamp() % server.Hosts.Length)];
            await client.ConnectAsync(new Uri($"wss://{serverHost.Host}:{serverHost.WssPort}/sub"), token);
            await SendJoinRoomAsync(client, roomId, 0, server.Token, token);
            _Client = client;
        }

        protected override ValueTask SendAsync(ReadOnlyMemory<byte> memory, CancellationToken token)
        {
            WebSocket? client;
            if (token.IsCancellationRequested || (client = _Client) == null)
            {
                return new ValueTask(Task.FromCanceled(token));
            }
            return client.SendAsync(memory, WebSocketMessageType.Binary, true, token);
        }

        protected override ValueTask ReceiveAsync(Memory<byte> memory, CancellationToken token)
        {
            WebSocket? client;
            if (token.IsCancellationRequested || (client = _Client) == null)
            {
                return new ValueTask(Task.FromCanceled(token));
            }
            return client.ReceiveFullyAsync(memory, token);
        }

        protected ValueTask SendJoinRoomAsync(WebSocket client, int roomId, int userId, string token, CancellationToken cToken = default)
        {
            return client.SendAsync(new ReadOnlyMemory<byte>(CreateJoinRoomPayload(roomId, userId, token)), WebSocketMessageType.Binary, true, cToken);
        }

        protected override void InternalDisconnect()
        {

        }

        protected override void InternalDispose(bool disposing)
        {

        }
    }
}
