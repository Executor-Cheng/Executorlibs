using System.Threading;
using Executorlibs.Huya.Protocol.Invokers;
using Executorlibs.Huya.Protocol.Options;
using Microsoft.Extensions.Options;

namespace Executorlibs.Huya.Protocol.Clients
{
    public abstract class HuyaClientBase
    {
        protected CancellationTokenSource? _cts = new();

        protected CancellationTokenSource? _workerCts;

        protected IHuyaMessageHandlerInvoker _invoker;

        protected IHuyaMessageSubscriptionResolver _resolver;

        protected HuyaClientOptions _options;

        protected HuyaClientBase(IHuyaMessageHandlerInvoker invoker, IHuyaMessageSubscriptionResolver resolver, IOptionsSnapshot<HuyaClientOptions> options)
        {
            _invoker = invoker;
            _resolver = resolver;
            _options = options.Value;
        }
    }
}
