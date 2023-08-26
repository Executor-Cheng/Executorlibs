using System;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Executorlibs.MessageFramework.Extensions;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Subscriptions;

namespace Executorlibs.MessageFramework.Builders
{
    public class DefaultMessageDispatcherBuilder<TClient, TMessage> where TClient : class, IMessageClient where TMessage : IMessage
    {
        protected readonly MessageFrameworkBuilder<TClient, TMessage> _builder;

        protected readonly MessageDispatcherServiceBuilder<TClient, TMessage, IMessageDispatcher<TClient, TMessage>> _dispatcherBuilder;

        public MessageFrameworkBuilder<TClient, TMessage> Builder => _builder;

        public DefaultMessageDispatcherBuilder(MessageFrameworkBuilder<TClient, TMessage> builder) : this(builder, builder.WithDefaultDispatcher())
        {

        }

        public DefaultMessageDispatcherBuilder(DefaultMessageDispatcherBuilder<TClient, TMessage> builder) : this(builder._builder, builder._dispatcherBuilder)
        {

        }

        public DefaultMessageDispatcherBuilder(MessageFrameworkBuilder<TClient, TMessage> builder, MessageDispatcherServiceBuilder<TClient, TMessage, IMessageDispatcher<TClient, TMessage>> dispatcherBuilder)
        {
            _builder = builder;
            _dispatcherBuilder = dispatcherBuilder;
        }

        public DefaultMessageDispatcherBuilder<TClient, TMessage> Configure(Action<SubscriptionServiceBuilder<TClient, TMessage, IMessageSubscription<TClient, TMessage>>> subscriptionBuilder)
        {
            subscriptionBuilder.Invoke(_builder.WithDefaultSubscription());
            _dispatcherBuilder.AddDefaultDispatcher();
            return this;
        }
    }

    public class DefaultMessageDispatcherBuilder<TClient, TMessage, TSubscriptionBuilder> : DefaultMessageDispatcherBuilder<TClient, TMessage> where TClient : class, IMessageClient where TMessage : IMessage where TSubscriptionBuilder : DefaultSubscriptionBuilder<TClient, TMessage>
    {
        protected readonly TSubscriptionBuilder _subscriptionBuilder;

        public DefaultMessageDispatcherBuilder<TClient, TMessage> Base => this;

        public DefaultMessageDispatcherBuilder(DefaultMessageDispatcherBuilder<TClient, TMessage> builder, TSubscriptionBuilder subscriptionBuilder) : base(builder)
        {
            _subscriptionBuilder = subscriptionBuilder;
        }

        public DefaultMessageDispatcherBuilder(DefaultMessageDispatcherBuilder<TClient, TMessage, TSubscriptionBuilder> builder) : this(builder, builder._subscriptionBuilder)
        {

        }

        public DefaultMessageDispatcherBuilder<TClient, TMessage, TSubscriptionBuilder> Configure(Action<HandlerServiceBuilder<TClient, TMessage, IMessageHandler<TClient, TMessage>>> handlerBuilder)
        {
            _subscriptionBuilder.Configure(handlerBuilder);
            _dispatcherBuilder.AddDefaultDispatcher();
            return this;
        }
    }
}
