using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{

    public class PrioritizedParsingContextBuilder<TClient, TRawdata> : DefaultParsingContextBuilder<TClient, TRawdata> where TClient : class, IMessageClient
    {
        public PrioritizedParsingContextBuilder(DefaultParsingContextBuilder<TClient, TRawdata> contextBuilder) : base(contextBuilder)
        {
            
        }

        protected override void ConfigureDispatcher<TMessage>(MessageFrameworkBuilder<TClient, TMessage> builder, ServiceLifetime? lifetime)
        {
            var dispatcherBuilder = builder.WithDispatcher<IMessageDispatcher<TClient, TMessage>>();
            dispatcherBuilder.AddService<PrioritizedMessageDispatcher<TClient, TMessage>>(lifetime);
        }
    }
}
