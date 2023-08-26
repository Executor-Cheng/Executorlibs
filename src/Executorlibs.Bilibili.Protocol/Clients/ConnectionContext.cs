using System;
using System.Threading;
using Executorlibs.Bilibili.Protocol.Options;

namespace Executorlibs.Bilibili.Protocol.Clients
{
    public abstract class ConnectionContext : IDisposable
    {
        public readonly CancellationTokenSource ConnectionCts;

        public uint RoomId;

        public TimeSpan HeartbeatInterval;

        private volatile int _flag;

        public bool Connected => _flag > 0;

        protected ConnectionContext(DanmakuClientOptions options)
        {
            ConnectionCts = new CancellationTokenSource();
            RoomId = options.RoomId;
            HeartbeatInterval = options.HeartbeatInterval;
            _flag = 0;
        }

        public void ClientConnected()
        {
#if !NETSTANDARD
            Interlocked.Or(ref _flag, 1);
#else
                int current = _flag;
                while (true)
                {
                    int newValue = current | 1;
                    int oldValue = Interlocked.CompareExchange(ref _flag, newValue, current);
                    if (oldValue == current)
                    {
                        return;
                    }
                    current = oldValue;
                }
#endif
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ConnectionCts.Cancel();
                ConnectionCts.Dispose();
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
}
