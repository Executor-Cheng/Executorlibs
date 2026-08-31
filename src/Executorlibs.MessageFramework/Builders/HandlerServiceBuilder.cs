using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class HandlerServiceBuilder<TClient, TMessage, THandler> : MessagingEnumerableComponentBuilder<TClient, TMessage, THandler>
                                                                      where TClient : class, IMessageClient
                                                                      where TMessage : IMessage
                                                                      where THandler : class, IMessageHandler<TClient, TMessage>
    {
        protected override ServiceLifetime DefaultLifetime => ServiceLifetime.Scoped;

        public HandlerServiceBuilder(MessageFrameworkBuilder<TClient, TMessage> builder) : base(builder)
        {
            
        }

        public HandlerServiceBuilder(HandlerServiceBuilder<TClient, TMessage, THandler> builder) : base(builder)
        {
            
        }
    }
}
