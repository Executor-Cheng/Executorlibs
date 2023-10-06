using System;
using Executorlibs.MessageFramework.Builders;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Parsing.Context;
using Executorlibs.MessageFramework.Parsing.Parsers;
using Executorlibs.MessageFramework.Subscriptions;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Extensions
{
    public static class MessageFrameworkBuilderExtensions
    {
        public static MessageFrameworkBuilder<TClient> AddMessageFramework<TClient>(this IServiceCollection services) where TClient : class, IMessageClient
        {
            return new MessageFrameworkBuilder<TClient>(services);
        }

        public static MessageDispatcherServiceBuilder<TClient, TMessage, IMessageDispatcher<TClient, TMessage>> WithDefaultDispatcher<TClient, TMessage>(this MessageFrameworkBuilder<TClient, TMessage> builder) where TClient : class, IMessageClient where TMessage : IMessage
        {
            return builder.WithDispatcher<IMessageDispatcher<TClient, TMessage>>();
        }

        public static HandlerServiceBuilder<TClient, TMessage, IMessageHandler<TClient, TMessage>> WithDefaultHandler<TClient, TMessage>(this MessageFrameworkBuilder<TClient, TMessage> builder) where TClient : class, IMessageClient where TMessage : IMessage
        {
            return builder.WithHandler<IMessageHandler<TClient, TMessage>>();
        }

        public static SubscriptionServiceBuilder<TClient, TMessage, IMessageSubscription<TClient, TMessage>> WithDefaultSubscription<TClient, TMessage>(this MessageFrameworkBuilder<TClient, TMessage> builder) where TClient : class, IMessageClient where TMessage : IMessage
        {
            return builder.WithSubscription<IMessageSubscription<TClient, TMessage>>();
        }

        public static RawdataDispatcherServiceBuilder<TClient, TRawdata, IRawdataDispatcher<TClient, TRawdata>> WithDefaultDispatcher<TClient, TRawdata>(this ParsingServiceBuilder<TClient, TRawdata> builder) where TClient : class, IMessageClient
        {
            return builder.WithDispatcher<IRawdataDispatcher<TClient, TRawdata>>();
        }

        public static ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>> WithDefaultParsingContext<TClient, TRawdata>(this ParsingServiceBuilder<TClient, TRawdata> builder) where TClient : class, IMessageClient
        {
            return builder.WithParsingContext<IParsingContext<TClient, TRawdata>>();
        }

        public static ParserServiceBuilder<TClient, TRawdata, IMessageParser<TClient, TRawdata>> WithDefaultParser<TClient, TRawdata>(this ParsingServiceBuilder<TClient, TRawdata> builder) where TClient : class, IMessageClient
        {
            return builder.WithParser<IMessageParser<TClient, TRawdata>>();
        }

        public static MessageFrameworkBuilder<TClient, TMessage> AddDefaultDispatcher<TClient, TMessage>(this MessageFrameworkBuilder<TClient, TMessage> builder, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient where TMessage : IMessage
        {
            builder.WithDefaultDispatcher().AddDefaultDispatcher(lifetime);
            return builder;
        }

        public static MessageDispatcherServiceBuilder<TClient, TMessage, IMessageDispatcher<TClient, TMessage>> AddDefaultDispatcher<TClient, TMessage>(this MessageDispatcherServiceBuilder<TClient, TMessage, IMessageDispatcher<TClient, TMessage>> builder, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient where TMessage : IMessage
        {
            builder.AddComponent<DefaultMessageDispatcher<TClient, TMessage>>(lifetime);
            return builder;
        }

        public static SubscriptionServiceBuilder<TClient, TMessage, IMessageSubscription<TClient, TMessage>> AddDefaultSubscription<TClient, TMessage>(this SubscriptionServiceBuilder<TClient, TMessage, IMessageSubscription<TClient, TMessage>> builder, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient where TMessage : IMessage
        {
            builder.AddComponent<DefaultMessageSubscription<TClient, TMessage>>(lifetime);
            return builder;
        }

        public static RawdataDispatcherServiceBuilder<TClient, TRawdata, IRawdataDispatcher<TClient, TRawdata>> AddDefaultDispatcher<TClient, TRawdata>(this RawdataDispatcherServiceBuilder<TClient, TRawdata, IRawdataDispatcher<TClient, TRawdata>> builder, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient
        {
            builder.AddComponent<DefaultRawdataDispatcher<TClient, TRawdata>>(lifetime);
            return builder;
        }
    }

    public static class DefaultMessageFrameworkBuilderExtensions
    {
        public static void AddDefault<TClient, TMessage>(this MessageDispatcherServiceBuilder<TClient, TMessage, IMessageDispatcher<TClient, TMessage>> dispatcherBuilder, Action<SubscriptionServiceBuilder<TClient, TMessage, IMessageSubscription<TClient, TMessage>>> subscriptionBuilderAction, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient where TMessage : IMessage
        {
            var subscriptionBuilder = dispatcherBuilder.Builder.WithDefaultSubscription();
            subscriptionBuilderAction.Invoke(subscriptionBuilder);
            dispatcherBuilder.AddDefaultDispatcher(lifetime);
        }

        public static void AddDefault<TClient, TMessage>(this SubscriptionServiceBuilder<TClient, TMessage, IMessageSubscription<TClient, TMessage>> subscription, Action<HandlerServiceBuilder<TClient, TMessage, IMessageHandler<TClient, TMessage>>> handlerBuilderAction, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient where TMessage : IMessage
        {
            var handlerBuilder = subscription.Builder.WithDefaultHandler();
            handlerBuilderAction.Invoke(handlerBuilder);
            subscription.AddDefaultSubscription(lifetime);
        }

        public static void AddDefault<TClient, TRawdata>(this RawdataDispatcherServiceBuilder<TClient, TRawdata, IRawdataDispatcher<TClient, TRawdata>> dispatcher, Action<ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>>> parsingContextBuilderAction, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient
        {
            var parsingContextBuilder = dispatcher.Builder.WithDefaultParsingContext();
            parsingContextBuilderAction.Invoke(parsingContextBuilder);
            dispatcher.AddDefaultDispatcher(lifetime);
        }

        public static DefaultParsingContextBuilder<TClient, TRawdata> WithDefault<TClient, TRawdata>(this ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>> context) where TClient : class, IMessageClient
        {
            return new DefaultParsingContextBuilder<TClient, TRawdata>(context);
        }

        public static DefaultParsingContextBuilder<TClient, TRawdata>.DefaultDispatcherBuilder WithDefaultDispatcher<TClient, TRawdata>(this DefaultParsingContextBuilder<TClient, TRawdata> builder, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient
        {
            return new DefaultParsingContextBuilder<TClient, TRawdata>.DefaultDispatcherBuilder(builder, lifetime);
        }

        public static DefaultParsingContextBuilder<TClient, TRawdata>.DefaultDispatcherBuilder.DefaultSubscriptionBuilder WithDefaultSubscription<TClient, TRawdata>(this DefaultParsingContextBuilder<TClient, TRawdata>.DefaultDispatcherBuilder builder, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient
        {
            return new DefaultParsingContextBuilder<TClient, TRawdata>.DefaultDispatcherBuilder.DefaultSubscriptionBuilder(builder, lifetime);
        }
    }
}
