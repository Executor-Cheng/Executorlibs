using System.Net.Sockets;
using Executorlibs.Bilibili.Protocol.Options;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public class TcpConnectionContext : ConnectionContext
    {
        public readonly Socket Socket;

        public TcpConnectionContext(DanmakuClientOptions options) : base(options)
        {
            Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Socket.SendTimeout = Socket.ReceiveTimeout = (int)HeartbeatInterval.TotalMilliseconds + 10000;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Socket.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
