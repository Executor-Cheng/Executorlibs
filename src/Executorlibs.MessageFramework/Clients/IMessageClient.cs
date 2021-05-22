using System;
using System.Threading;
using System.Threading.Tasks;

namespace Executorlibs.MessageFramework.Clients
{
    public interface IMessageClient : IDisposable
    {
        bool Connected { get; }

        Task ConnectAsync(CancellationToken token = default);

        void Disconnect();
    }
}
