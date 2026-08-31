using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Executorlibs.MessageFramework.Models.General;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class MessageDispatcherServiceBuilder<TClient, TMessage, TDispatcher> : MessagingComponentBuilder<TClient, TMessage, TDispatcher>
                                                                                   where TClient : class, IMessageClient
                                                                                   where TMessage : IMessage
                                                                                   where TDispatcher : class, IMessageDispatcher<TClient, TMessage>
    {
        protected override ServiceLifetime DefaultLifetime => ServiceLifetime.Scoped;

        public MessageDispatcherServiceBuilder(MessageFrameworkBuilder<TClient, TMessage> builder) : base(builder)
        {
            
        }

        public MessageDispatcherServiceBuilder(MessageDispatcherServiceBuilder<TClient, TMessage, TDispatcher> builder) : base(builder)
        {
            
        }
    }
}
