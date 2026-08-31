using System.Net.WebSockets;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public abstract class DanmakuTransportFactory<TTransport> where TTransport : DanmakuTransport
    {
        public abstract TTransport Create();
    }

    public class TcpDanmakuTransportFactory : DanmakuTransportFactory<TcpDanmakuTransport>
    {
        public override TcpDanmakuTransport Create()
        {
            return new TcpDanmakuTransport();
        }
    }

    public class ClientWebSocketDanmakuTransportFactory : DanmakuTransportFactory<WebSocketDanmakuTransport<ClientWebSocket>>
    {
        public override WebSocketDanmakuTransport<ClientWebSocket> Create()
        {
            return new WebSocketDanmakuTransport<ClientWebSocket>(new ClientWebSocket());
        }
    }
}
