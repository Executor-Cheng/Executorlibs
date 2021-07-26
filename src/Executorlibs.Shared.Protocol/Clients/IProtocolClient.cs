using System.Threading;
using System.Threading.Tasks;
using Executorlibs.MessageFramework.Clients;

namespace Executorlibs.Shared.Protocol.Clients
{
    public interface IProtocolClient : IMessageClient
    {
        bool Connected { get; }

        Task ConnectAsync(CancellationToken token = default);

        void Disconnect();
    }
}
