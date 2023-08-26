using System;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Extensions;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Subscriptions;

namespace Executorlibs.MessageFramework.Builders
{
    public class DefaultSubscriptionBuilder<TClient, TMessage> where TClient : class, IMessageClient where TMessage : IMessage
    {
        protected readonly MessageFrameworkBuilder<TClient, TMessage> _builder;

        protected readonly SubscriptionServiceBuilder<TClient, TMessage, IMessageSubscription<TClient, TMessage>> _subscriptionBuilder;

        public MessageFrameworkBuilder<TClient, TMessage> Builder => _builder;

        public DefaultSubscriptionBuilder(MessageFrameworkBuilder<TClient, TMessage> builder) : this(builder, builder.WithDefaultSubscription())
        {
            
        }

        public DefaultSubscriptionBuilder(MessageFrameworkBuilder<TClient, TMessage> builder, SubscriptionServiceBuilder<TClient, TMessage, IMessageSubscription<TClient, TMessage>> subscriptionBuilder)
        {
            _builder = builder;
            _subscriptionBuilder = subscriptionBuilder;
        }

        public DefaultSubscriptionBuilder<TClient, TMessage> Configure(Action<HandlerServiceBuilder<TClient, TMessage, IMessageHandler<TClient, TMessage>>> handlerBuilder)
        {
            handlerBuilder.Invoke(_builder.WithDefaultHandler());
            _subscriptionBuilder.AddDefaultSubscription();
            return this;
        }
    }
}
