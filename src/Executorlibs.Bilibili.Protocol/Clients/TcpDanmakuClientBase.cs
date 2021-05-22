using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
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
    public abstract class TcpDanmakuClientBase : DanmakuClientBase
    {
        protected volatile Socket? _Socket;

        public override bool Connected => base.Connected && (_Socket?.Connected == true);

        protected TcpDanmakuClientBase(IBilibiliMessageHandlerInvoker invoker, IOptionsSnapshot<DanmakuClientOptions> options, IDanmakuServerProvider credentialProvider) : base(invoker, options, credentialProvider)
        {

        }

        protected override ValueTask SendAsync(Memory<byte> memory, CancellationToken token)
        {
            Socket? socket;
            if (token.IsCancellationRequested || (socket = _Socket) == null)
            {
                return new ValueTask(Task.FromCanceled(token));
            }
            return new ValueTask(socket.SendAsync(memory, SocketFlags.None, token).AsTask());
        }

        protected override ValueTask ReceiveAsync(Memory<byte> memory, CancellationToken token)
        {
            Socket? socket;
            if (token.IsCancellationRequested || (socket = _Socket) == null)
            {
                return new ValueTask(Task.FromCanceled(token));
            }
            return socket.ReceiveFullyAsync(memory, token);
        }

        protected override async Task InternalConnectAsync(CancellationToken token)
        {
            int roomId = _Options.RoomId;
            DanmakuServerInfo server = await _CredentialProvider.GetDanmakuServerInfoAsync(token);
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.SendTimeout = socket.ReceiveTimeout = (int)_Options.HeartbeatInterval.TotalMilliseconds + 10000;
            token.Register(socket.Dispose);
            DanmakuServerHostInfo serverHost = server.Hosts[(int)(Stopwatch.GetTimestamp() % server.Hosts.Length)];
#if NET5_0_OR_GREATER
            await socket.ConnectAsync(serverHost.Host, serverHost.Port, token);
#else
            await socket.ConnectAsync(serverHost.Host, serverHost.Port);
#endif
            await SendJoinRoomAsync(socket, roomId, 0,server.Token, token);
            _Socket = socket;
        }

        protected override void InternalDisconnect()
        {
            
        }

        protected override void InternalDispose(bool disposing)
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected ValueTask SendJoinRoomAsync(Socket socket, int roomId, int userId, string token, CancellationToken cToken = default)
        {
            return new ValueTask(socket.SendAsync(CreateJoinRoomPayload(roomId, userId, token), SocketFlags.None, cToken).AsTask());
        }
    }
}
