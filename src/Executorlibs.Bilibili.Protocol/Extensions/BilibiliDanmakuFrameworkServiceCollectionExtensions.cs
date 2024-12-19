using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Builders;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Dispatchers;
using Executorlibs.Bilibili.Protocol.Handlers;
using Executorlibs.Bilibili.Protocol.Models.Danmaku;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Parsing.Contexts;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Subscriptions;
using Executorlibs.MessageFramework.Builders;
using Executorlibs.MessageFramework.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.Bilibili.Protocol.Extensions
{
    public static class BilibiliDanmakuFrameworkServiceCollectionExtensions
    {
        public static ParsingServiceBuilder<IDanmakuClient, byte[]> AddBilibiliDanmakuFramework(this IServiceCollection services)
        {
            return services.AddMessageFramework<IDanmakuClient>().WithRawdata<byte[]>();
        }

        public static MessageDispatcherServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageDispatcher<TMessage>> WithDefaultDispatcher<TMessage>(this MessageFrameworkBuilder<IDanmakuClient, TMessage> builder) where TMessage : IBilibiliMessage
        {
            return builder.WithDispatcher<IBilibiliMessageDispatcher<TMessage>>();
        }

        public static HandlerServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageHandler<TMessage>> WithDefaultHandler<TMessage>(this MessageFrameworkBuilder<IDanmakuClient, TMessage> builder) where TMessage : IBilibiliMessage
        {
            return builder.WithHandler<IBilibiliMessageHandler<TMessage>>();
        }

        public static SubscriptionServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageSubscription<TMessage>> WithDefaultSubscription<TMessage>(this MessageFrameworkBuilder<IDanmakuClient, TMessage> builder) where TMessage : IBilibiliMessage
        {
            return builder.WithSubscription<IBilibiliMessageSubscription<TMessage>>();
        }

        public static ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext> WithDefaultParsingContext(this ParsingServiceBuilder<IDanmakuClient, byte[]> builder)
        {
            return builder.WithParsingContext<IBilibiliRawParsingContext>();
        }

        public static RawdataDispatcherServiceBuilder<IDanmakuClient, byte[], IBilibiliRawdataDispatcher> WithDefaultDispatcher(this ParsingServiceBuilder<IDanmakuClient, byte[]> builder)
        {
            return builder.WithDispatcher<IBilibiliRawdataDispatcher>();
        }

        public static RawdataDispatcherServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonDispatcher> WithDefaultDispatcher(this ParsingServiceBuilder<IDanmakuClient, JsonElement> builder)
        {
            return builder.WithDispatcher<IBilibiliJsonDispatcher>();
        }

        public static ParsingContextServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonParsingContext> WithDefaultParsingContext(this ParsingServiceBuilder<IDanmakuClient, JsonElement> builder)
        {
            return builder.WithParsingContext<IBilibiliJsonParsingContext>();
        }

        public static ParsingServiceBuilder<IDanmakuClient, byte[]> AddDefaultRawdataDispatcher(this ParsingServiceBuilder<IDanmakuClient, byte[]> builder, Action<ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext>>? parsingBuilderAction, ServiceLifetime? lifetime = null)
        {
            if (parsingBuilderAction != null)
            {
                var parsingContextBuilder = builder.WithDefaultParsingContext();
                parsingBuilderAction.Invoke(parsingContextBuilder);
            }
            builder.WithDefaultDispatcher().AddDefaultDispatcher(lifetime);
            return builder;
        }

        public static RawdataDispatcherServiceBuilder<IDanmakuClient, byte[], IBilibiliRawdataDispatcher> AddDefaultDispatcher(this RawdataDispatcherServiceBuilder<IDanmakuClient, byte[], IBilibiliRawdataDispatcher> builder, ServiceLifetime? lifetime = null)
        {
            builder.AddComponent<BilibiliRawdataDispatcher>(lifetime);
            return builder;
        }

        public static RawdataDispatcherServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonDispatcher> AddDefaultDispatcher(this RawdataDispatcherServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonDispatcher> builder, ServiceLifetime? lifetime = null)
        {
            builder.AddComponent<BilibiliJsonDispatcher>(lifetime);
            return builder;
        }

        public static SubscriptionServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageSubscription<TMessage>> AddDefaultSubscription<TMessage>(this SubscriptionServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageSubscription<TMessage>> builder, ServiceLifetime? lifetime = null) where TMessage : IBilibiliMessage
        {
            builder.AddComponent<BilibiliMessageSubscription<TMessage>>(lifetime);
            return builder;
        }

        public static MessageDispatcherServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageDispatcher<TMessage>> AddDefaultDispatcher<TMessage>(this MessageDispatcherServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageDispatcher<TMessage>> builder, ServiceLifetime? lifetime = null) where TMessage : IBilibiliMessage
        {
            builder.AddComponent<BilibiliMessageDispatcher<TMessage>>(lifetime);
            return builder;
        }

        public static DanmakuCredentialProviderBuilder WithDanmakuCredentialProvider(this ParsingServiceBuilder<IDanmakuClient, byte[]> builder)
        {
            return new DanmakuCredentialProviderBuilder(builder);
        }
    }

    public static class DefaultBilibiliMessageFrameworkExtensions
    {
        public static void AddDefault<TMessage>(this MessageDispatcherServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageDispatcher<TMessage>> dispatcherBuilder, Action<SubscriptionServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageSubscription<TMessage>>> subscriptionBuilderAction, ServiceLifetime? lifetime = null) where TMessage : IBilibiliMessage
        {
            var subscriptionBuilder = dispatcherBuilder.Builder.WithDefaultSubscription();
            subscriptionBuilderAction.Invoke(subscriptionBuilder);
            dispatcherBuilder.AddDefaultDispatcher(lifetime);
        }

        public static void AddDefault<TMessage>(this SubscriptionServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageSubscription<TMessage>> subscription, Action<HandlerServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageHandler<TMessage>>> handlerBuilderAction, ServiceLifetime? lifetime = null) where TMessage : IBilibiliMessage
        {
            var handlerBuilder = subscription.Builder.WithDefaultHandler();
            handlerBuilderAction.Invoke(handlerBuilder);
            subscription.AddDefaultSubscription(lifetime);
        }

        public static void AddDefault(this RawdataDispatcherServiceBuilder<IDanmakuClient, byte[], IBilibiliRawdataDispatcher> dispatcher, Action<ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext>> parsingContextBuilderAction, ServiceLifetime? lifetime = null)
        {
            var parsingContextBuilder = dispatcher.Builder.WithDefaultParsingContext();
            parsingContextBuilderAction.Invoke(parsingContextBuilder);
            dispatcher.AddDefaultDispatcher(lifetime);
        }

        public static void AddDefault(this RawdataDispatcherServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonDispatcher> dispatcher, Action<ParsingContextServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonParsingContext>> parsingContextBuilderAction, ServiceLifetime? lifetime = null)
        {
            var parsingContextBuilder = dispatcher.Builder.WithDefaultParsingContext();
            parsingContextBuilderAction.Invoke(parsingContextBuilder);
            dispatcher.AddDefaultDispatcher(lifetime);
        }

        public static ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext> TransistToJson(this ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext> builder, Action<RawdataDispatcherServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonDispatcher>> dispatcherBuilderAction, ServiceLifetime? lifetime = null)
        {
            var dispatcherBuilder = builder.Builder.WithRawdata<JsonElement>().WithDefaultDispatcher();
            dispatcherBuilderAction.Invoke(dispatcherBuilder);
            builder.AddComponent<BilibiliRawToJsonParsingContext>(lifetime);
            return builder;
        }

        public static DefaultRawParsingContextBuilder WithDefault(this ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext> builder)
        {
            return new DefaultRawParsingContextBuilder(builder);
        }

        public static DefaultRawParsingContextBuilder.DefaultDispatcherBuilder WithDefaultDispatcher(this DefaultRawParsingContextBuilder builder, ServiceLifetime? lifetime = null)
        {
            return new DefaultRawParsingContextBuilder.DefaultDispatcherBuilder(builder, lifetime);
        }

        public static DefaultRawParsingContextBuilder.DefaultDispatcherBuilder.DefaultSubscriptionBuilder WithDefaultSubscription(this DefaultRawParsingContextBuilder.DefaultDispatcherBuilder builder, ServiceLifetime? lifetime = null)
        {
            return new DefaultRawParsingContextBuilder.DefaultDispatcherBuilder.DefaultSubscriptionBuilder(builder, lifetime);
        }

        public static DefaultJsonParsingContextBuilder WithDefault(this ParsingContextServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonParsingContext> builder)
        {
            return new DefaultJsonParsingContextBuilder(builder);
        }

        public static DefaultJsonParsingContextBuilder.DefaultDispatcherBuilder AddDefaultDispatcher(this DefaultJsonParsingContextBuilder builder, ServiceLifetime? lifetime = null)
        {
            return new DefaultJsonParsingContextBuilder.DefaultDispatcherBuilder(builder, lifetime);
        }

        public static DefaultJsonParsingContextBuilder.DefaultDispatcherBuilder.DefaultSubscriptionBuilder AddDefaultSubscription(this DefaultJsonParsingContextBuilder.DefaultDispatcherBuilder builder, ServiceLifetime? lifetime = null)
        {
            return new DefaultJsonParsingContextBuilder.DefaultDispatcherBuilder.DefaultSubscriptionBuilder(builder, lifetime);
        }
    }
}
