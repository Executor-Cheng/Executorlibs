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

        public static MessageFrameworkBuilder<TClient, TMessage> ConfigureDefaults<TClient, TMessage>(this MessageFrameworkBuilder<TClient, TMessage> builder) where TClient : class, IMessageClient where TMessage : IMessage
        {
            builder.WithDefaultDispatcher().AddDefaultDispatcher();
            builder.WithDefaultSubscription().AddDefaultSubscription();
            return builder;
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

        public static MessageFrameworkBuilder<TClient, TMessage> AddDefaultDispatcher<TClient, TMessage>(this MessageFrameworkBuilder<TClient, TMessage> builder) where TClient : class, IMessageClient where TMessage : IMessage
        {
            builder.WithDefaultDispatcher().AddDefaultDispatcher();
            return builder;
        }

        public static MessageDispatcherServiceBuilder<TClient, TMessage, IMessageDispatcher<TClient, TMessage>> AddDefaultDispatcher<TClient, TMessage>(this MessageDispatcherServiceBuilder<TClient, TMessage, IMessageDispatcher<TClient, TMessage>> builder) where TClient : class, IMessageClient where TMessage : IMessage
        {
            builder.AddComponent<DefaultMessageDispatcher<TClient, TMessage>>();
            return builder;
        }

        public static SubscriptionServiceBuilder<TClient, TMessage, IMessageSubscription<TClient, TMessage>> AddDefaultSubscription<TClient, TMessage>(this SubscriptionServiceBuilder<TClient, TMessage, IMessageSubscription<TClient, TMessage>> builder) where TClient : class, IMessageClient where TMessage : IMessage
        {
            builder.AddComponent<DefaultMessageSubscription<TClient, TMessage>>();
            return builder;
        }

        public static RawdataDispatcherServiceBuilder<TClient, TRawdata, IRawdataDispatcher<TClient, TRawdata>> AddDefaultDispatcher<TClient, TRawdata>(this RawdataDispatcherServiceBuilder<TClient, TRawdata, IRawdataDispatcher<TClient, TRawdata>> builder) where TClient : class, IMessageClient
        {
            builder.AddComponent<DefaultRawdataDispatcher<TClient, TRawdata>>();
            return builder;
        }
    }

    public static class DefaultMessageFrameworkExtensions
    {
        public static DefaultMessageDispatcherBuilder<TClient, TMessage> AddDefaultDispatcher<TClient, TMessage>(this MessageFrameworkBuilder<TClient, TMessage> builder) where TClient : class, IMessageClient where TMessage : IMessage
        {
            return new DefaultMessageDispatcherBuilder<TClient, TMessage>(builder);
        }

        public static DefaultMessageDispatcherBuilder<TClient, TMessage, DefaultSubscriptionBuilder<TClient, TMessage>> UseDefaultSubscription<TClient, TMessage>(this DefaultMessageDispatcherBuilder<TClient, TMessage> builder) where TClient : class, IMessageClient where TMessage : IMessage
        {
            return new DefaultMessageDispatcherBuilder<TClient, TMessage, DefaultSubscriptionBuilder<TClient, TMessage>>(builder, new DefaultSubscriptionBuilder<TClient, TMessage>(builder.Builder));
        }

        public static DefaultParsingContextBuilder<TClient, TRawdata> AddDefaultDispatcher<TClient, TRawdata>(this ParsingServiceBuilder<TClient, TRawdata> builder) where TClient : class, IMessageClient
        {
            builder.WithDefaultDispatcher().AddDefaultDispatcher();
            return new DefaultParsingContextBuilder<TClient, TRawdata>(builder);
        }
    }
}
