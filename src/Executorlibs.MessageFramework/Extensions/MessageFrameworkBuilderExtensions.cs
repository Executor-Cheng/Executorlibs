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

        public static RawdataDispatcherServiceBuilder<TClient, TRawdata, IRawdataDispatcher<TClient, TRawdata>> WithDefaultDispatcher<TClient, TRawdata>(this ParsingServiceBuilder<TClient, TRawdata> builder) where TClient : class, IMessageClient
        {
            return builder.WithDispatcher<IRawdataDispatcher<TClient, TRawdata>>();
        }

        public static ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>> WithDefaultParsingContext<TClient, TRawdata>(this ParsingServiceBuilder<TClient, TRawdata> builder) where TClient : class, IMessageClient
        {
            return builder.WithParsingContext<IParsingContext<TClient, TRawdata>>();
        }

        public static ParserServiceBuilder<TClient, TRawdata, IMessageParser<TRawdata>> WithDefaultParser<TClient, TRawdata>(this ParsingServiceBuilder<TClient, TRawdata> builder) where TClient : class, IMessageClient
        {
            return builder.WithParser<IMessageParser<TRawdata>>();
        }

        public static MessageFrameworkBuilder<TClient, TMessage> AddDefaultDispatcher<TClient, TMessage>(this MessageFrameworkBuilder<TClient, TMessage> builder, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient where TMessage : IMessage
        {
            builder.WithDefaultDispatcher().AddDefaultDispatcher(lifetime);
            return builder;
        }

        public static MessageDispatcherServiceBuilder<TClient, TMessage, IMessageDispatcher<TClient, TMessage>> AddDefaultDispatcher<TClient, TMessage>(this MessageDispatcherServiceBuilder<TClient, TMessage, IMessageDispatcher<TClient, TMessage>> builder, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient where TMessage : IMessage
        {
            builder.AddService<DefaultMessageDispatcher<TClient, TMessage>>(lifetime);
            return builder;
        }

        public static RawdataDispatcherServiceBuilder<TClient, TRawdata, IRawdataDispatcher<TClient, TRawdata>> AddDefaultDispatcher<TClient, TRawdata>(this RawdataDispatcherServiceBuilder<TClient, TRawdata, IRawdataDispatcher<TClient, TRawdata>> builder, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient
        {
            builder.AddService<DefaultRawdataDispatcher<TClient, TRawdata>>(lifetime);
            return builder;
        }
    }

    public static class DefaultParsingFrameworkBuilderExtensions
    {
        public static DefaultParsingContextBuilder<TClient, TRawdata> UseDefault<TClient, TRawdata>(this ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>> context) where TClient : class, IMessageClient
        {
            return new DefaultParsingContextBuilder<TClient, TRawdata>(context);
        }

        public static PrioritizedParsingContextBuilder<TClient, TRawdata> UsePriority<TClient, TRawdata>(this DefaultParsingContextBuilder<TClient, TRawdata> builder) where TClient : class, IMessageClient
        {
            return new PrioritizedParsingContextBuilder<TClient, TRawdata>(builder);
        }

        public static ParsingServiceBuilder<TClient, TRawdata> UseParsingContext<TClient, TRawdata>(this ParsingServiceBuilder<TClient, TRawdata> builder, Action<ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>>> parsingBuilderAction) where TClient : class, IMessageClient
        {
            var parsingContextBuilder = builder.WithDefaultParsingContext();
            parsingBuilderAction.Invoke(parsingContextBuilder);

            builder.WithDefaultDispatcher().AddDefaultDispatcher();
            return builder;
        }
    }

    public static class DefaultMessageFrameworkBuilderExtensions
    {
        public static DefaultDispatcherBuilder<TClient> UseDispatcher<TClient>(this MessageFrameworkBuilder<TClient> builder) where TClient : class, IMessageClient
        {
            return new DefaultDispatcherBuilder<TClient>(builder);
        }

        public static PrioritizedDispatcherBuilder<TClient> UsePriority<TClient>(this DefaultDispatcherBuilder<TClient> builder) where TClient : class, IMessageClient
        {
            return new PrioritizedDispatcherBuilder<TClient>(builder);
        }

        //public static void AddDefault<TClient, TMessage>(this MessageDispatcherServiceBuilder<TClient, TMessage, IMessageDispatcher<TClient, TMessage>> dispatcherBuilder, Action<SubscriptionServiceBuilder<TClient, TMessage, IMessageSubscription<TClient, TMessage>>> subscriptionBuilderAction, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient where TMessage : IMessage
        //{
        //    var subscriptionBuilder = dispatcherBuilder.Builder.WithDefaultSubscription();
        //    subscriptionBuilderAction.Invoke(subscriptionBuilder);
        //    dispatcherBuilder.AddDefaultDispatcher(lifetime);
        //}

        //public static void AddDefault<TClient, TMessage>(this SubscriptionServiceBuilder<TClient, TMessage, IMessageSubscription<TClient, TMessage>> subscription, Action<HandlerServiceBuilder<TClient, TMessage, IMessageHandler<TClient, TMessage>>> handlerBuilderAction, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient where TMessage : IMessage
        //{
        //    var handlerBuilder = subscription.Builder.WithDefaultHandler();
        //    handlerBuilderAction.Invoke(handlerBuilder);
        //    subscription.AddDefaultSubscription(lifetime);
        //}

        //public static void UseContext<TClient, TRawdata>(this RawdataDispatcherServiceBuilder<TClient, TRawdata, IRawdataDispatcher<TClient, TRawdata>> dispatcher, Action<ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>>> parsingContextBuilderAction, ServiceLifetime? lifetime = null) where TClient : class, IMessageClient
        //{
        //    var parsingContextBuilder = dispatcher.Builder.WithDefaultParsingContext();
        //    parsingContextBuilderAction.Invoke(parsingContextBuilder);
        //    dispatcher.AddDefaultDispatcher(lifetime);
        //}
    }
}
