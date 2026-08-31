using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Bilibili.Protocol.Models;
using Executorlibs.Bilibili.Protocol.Options;
using Executorlibs.Bilibili.Protocol.Services;

namespace Executorlibs.Bilibili.Protocol.Clients.Contexts
{
    public abstract class DanmakuClientContextFactory<TTransport> where TTransport : DanmakuTransport
    {
        protected readonly DanmakuTransportFactory<TTransport> _factory;

        protected readonly IDanmakuServerProvider _credentialProvider;

        protected DanmakuClientContextFactory(DanmakuTransportFactory<TTransport> factory, IDanmakuServerProvider credentialProvider)
        {
            _factory = factory;
            _credentialProvider = credentialProvider;
        }

        public async Task<DanmakuClientContext<TTransport>> CreateAsync(DanmakuClientOptions options, CancellationToken token = default)
        {
            var transport = _factory.Create();
            try
            {
                var serverInfo = await _credentialProvider.GetDanmakuServerInfoAsync(options.RoomId, token);
                await ConnectAsync(transport, serverInfo, token);
                return new DanmakuClientContext<TTransport>(transport, options, serverInfo);
            }
            catch
            {
                transport.Dispose();
                throw;
            }
        }

        protected abstract Task ConnectAsync(TTransport transport, DanmakuServerInfo serverInfo, CancellationToken token);
    }

    public class TcpDanmakuClientTransportFactory : DanmakuClientContextFactory<TcpDanmakuTransport>
    {
        public TcpDanmakuClientTransportFactory(DanmakuTransportFactory<TcpDanmakuTransport> factory, IDanmakuServerProvider credentialProvider) : base(factory, credentialProvider)
        {

        }

        protected override Task ConnectAsync(TcpDanmakuTransport transport, DanmakuServerInfo serverInfo, CancellationToken token)
        {
            var hostInfo = serverInfo.Hosts[(int)(Stopwatch.GetTimestamp() % serverInfo.Hosts.Length)];
            return transport.Socket.ConnectAsync(hostInfo.Host, hostInfo.Port);
        }
    }

    public class WebSocketDanmakuClientContextFactory : DanmakuClientContextFactory<WebSocketDanmakuTransport<ClientWebSocket>>
    {
        public WebSocketDanmakuClientContextFactory(DanmakuTransportFactory<WebSocketDanmakuTransport<ClientWebSocket>> factory, IDanmakuServerProvider credentialProvider) : base(factory, credentialProvider)
        {

        }

        protected override Task ConnectAsync(WebSocketDanmakuTransport<ClientWebSocket> transport, DanmakuServerInfo serverInfo, CancellationToken token)
        {
            var hostInfo = serverInfo.Hosts[(int)(Stopwatch.GetTimestamp() % serverInfo.Hosts.Length)];
            return transport.WebSocket.ConnectAsync(new Uri($"wss://{hostInfo.Host}:{hostInfo.WssPort}/sub"), token);
        }
    }
}
