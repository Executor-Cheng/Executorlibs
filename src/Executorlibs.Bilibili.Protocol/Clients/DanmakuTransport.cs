using System;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Shared.Extensions;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    //public interface IDanmakuTransport : IDisposable
    //{
    //    bool Connected { get; }

    //    CancellationToken Token { get; }

    //    ValueTask ReceiveAsync(Memory<byte> memory, CancellationToken token);

    //    ValueTask SendAsync(ReadOnlyMemory<byte> memory, CancellationToken token);
    //}

    public abstract class DanmakuTransport //: IDanmakuTransport
    {
        protected readonly CancellationTokenSource _lifetime;

        protected volatile int _flag;

        public virtual bool Connected => _flag > 0;

        public CancellationToken Token => _lifetime.UnsafeGetToken();

        protected DanmakuTransport()
        {
            _lifetime = new CancellationTokenSource();
            _flag = 0;
        }

        public abstract ValueTask SendAsync(ReadOnlyMemory<byte> memory, CancellationToken token);

        public abstract ValueTask ReceiveAsync(Memory<byte> memory, CancellationToken token);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _lifetime.Cancel();
                _lifetime.Dispose();
            }
        }

        public void Dispose()
        {
            var flag = Interlocked.Exchange(ref _flag, -1);
            if (flag >= 0)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }
    }

    public class TcpDanmakuTransport : DanmakuTransport
    {
        public Socket Socket { get; }

        public TcpDanmakuTransport()
        {
            Socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public override ValueTask SendAsync(ReadOnlyMemory<byte> memory, CancellationToken token)
        {
            return Socket.SendAsync(memory, SocketFlags.None, token).DangerousAsValueTask();
        }

        public override ValueTask ReceiveAsync(Memory<byte> memory, CancellationToken token)
        {
            return Socket.ReceiveFullyAsync(memory, SocketFlags.None, token);
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

    public class WebSocketDanmakuTransport<TWebSocket> : DanmakuTransport where TWebSocket : WebSocket
    {
        public TWebSocket WebSocket { get; }

        public WebSocketDanmakuTransport(TWebSocket webSocket)
        {
            WebSocket = webSocket;
        }

        public override ValueTask SendAsync(ReadOnlyMemory<byte> memory, CancellationToken token)
        {
#if NETSTANDARD2_0
            return new ValueTask(WebSocket.SendAsync(memory, WebSocketMessageType.Binary, true, token));
#else
            return WebSocket.SendAsync(memory, WebSocketMessageType.Binary, true, token);
#endif
        }

        public override ValueTask ReceiveAsync(Memory<byte> memory, CancellationToken token)
        {
#if NETSTANDARD2_0
            return new ValueTask(WebSocket.ReceiveFullyAsync(memory, token));
#else
            return new ValueTask(WebSocket.ReceiveFullyAsync(memory, token));
#endif
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
