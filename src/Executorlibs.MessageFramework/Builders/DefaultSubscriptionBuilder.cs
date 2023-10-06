using System;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Subscriptions;

namespace Executorlibs.MessageFramework.Builders
{
    public readonly struct DefaultSubscriptionBuilder<TClient, TMessage, TSubscription, THandler> where TClient : class, IMessageClient
                                                                                                  where TMessage : IMessage
                                                                                                  where TSubscription : class, IMessageSubscription<TClient, TMessage>
                                                                                                  where THandler : class, IMessageHandler<TClient, TMessage>
    {
        private readonly MessageFrameworkBuilder<TClient, TMessage> _builder;

        public MessageFrameworkBuilder<TClient, TMessage> Builder => _builder;

        public DefaultSubscriptionBuilder(MessageFrameworkBuilder<TClient, TMessage> builder)
        {
            _builder = builder;
        }

        public DefaultSubscriptionBuilder(DefaultSubscriptionBuilder<TClient, TMessage, TSubscription, THandler> builder) : this(builder._builder)
        {

        }

        public DefaultSubscriptionBuilder<TClient, TMessage, TSubscription, THandler> Configure(Action<SubscriptionServiceBuilder<TClient, TMessage, TSubscription>> subscriptionBuilderAction, Action<HandlerServiceBuilder<TClient, TMessage, THandler>> handlerBuilderAction)
        {
            var subscriptionBuilder = _builder.WithSubscription<TSubscription>();
            subscriptionBuilderAction.Invoke(subscriptionBuilder);
            var handlerBuilder = _builder.WithHandler<THandler>();
            handlerBuilderAction.Invoke(handlerBuilder);
            return this;
        }
    }
}
