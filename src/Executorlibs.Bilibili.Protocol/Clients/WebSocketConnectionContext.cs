using System.Net.WebSockets;
using System.Threading;
using Executorlibs.Bilibili.Protocol.Options;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public class WebSocketConnectionContext : ConnectionContext
    {
        public readonly ClientWebSocket WebSocket;

        public WebSocketConnectionContext(DanmakuClientOptions options) : base(options)
        {
            var webSocket = new ClientWebSocket();
            webSocket.Options.KeepAliveInterval = Timeout.InfiniteTimeSpan;
            WebSocket = webSocket;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                WebSocket.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
