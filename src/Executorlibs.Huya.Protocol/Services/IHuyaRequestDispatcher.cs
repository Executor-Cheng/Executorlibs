using System.Threading;
using System.Threading.Tasks;
using Executorlibs.Huya.Protocol.Clients;
using Executorlibs.TarProtocol.Models;

namespace Executorlibs.Huya.Protocol.Services
{
    public interface IHuyaRequestDispatcher<TRequest, TResponse> where TRequest : ITarType where TResponse : ITarType
    {
        Task<TResponse> InvokeAsync(IHuyaClient client, TRequest request, CancellationToken token = default);

        Task<TResponse> InvokeAsync(IHuyaClient client, TRequest request, string servant, string function, CancellationToken token = default);
    }
}
