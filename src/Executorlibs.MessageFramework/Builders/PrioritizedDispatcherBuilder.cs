using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class PrioritizedDispatcherBuilder<TClient> : DefaultDispatcherBuilder<TClient> where TClient : class, IMessageClient
    {
        public PrioritizedDispatcherBuilder(DefaultDispatcherBuilder<TClient> builder) : base(builder)
        {

        }

        protected override void ConfigureDispatcher<TMessage>(MessageFrameworkBuilder<TClient, TMessage> builder, ServiceLifetime? lifetime)
        {
            var dispatcherBuilder = builder.WithDispatcher<IMessageDispatcher<TClient, TMessage>>();
            dispatcherBuilder.AddService<PrioritizedMessageDispatcher<TClient, TMessage>>(lifetime);
        }
    }
}
