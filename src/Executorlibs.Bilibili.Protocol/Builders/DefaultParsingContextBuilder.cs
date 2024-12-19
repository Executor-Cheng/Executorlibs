using System;
using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Dispatchers;
using Executorlibs.Bilibili.Protocol.Extensions;
using Executorlibs.Bilibili.Protocol.Handlers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Parsing.Contexts;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Subscriptions;
using Executorlibs.MessageFramework.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.Bilibili.Protocol.Builders
{
    public readonly struct DefaultRawParsingContextBuilder
    {
        private readonly ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext> _contextBuilder;

        public DefaultRawParsingContextBuilder(ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext> contextBuilder)
        {
            _contextBuilder = contextBuilder;
        }

        public DefaultRawParsingContextBuilder WithMessage<TMessage>(Action<ParserServiceBuilder<IDanmakuClient, byte[], IBilibiliRawMessageParser<TMessage>>> parserBuilderAction, Action<MessageDispatcherServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageDispatcher<TMessage>>> dispatcherBuilderAction, ServiceLifetime? lifetime = null) where TMessage : IBilibiliRawMessage
        {
            var contextBuilder = _contextBuilder;
            var builder = contextBuilder.Builder;

            var parserBuilder = builder.WithParser<IBilibiliRawMessageParser<TMessage>>();
            parserBuilderAction.Invoke(parserBuilder);

            var dispatcherBuilder = builder.WithMessage<TMessage>().WithDefaultDispatcher();
            dispatcherBuilderAction.Invoke(dispatcherBuilder);

            contextBuilder.AddComponent<BilibiliRawParsingContext<TMessage>>(lifetime);
            return this;
        }

        public readonly struct DefaultDispatcherBuilder
        {
            private readonly DefaultRawParsingContextBuilder _builder;

            private readonly ServiceLifetime? _lifetime;

            public DefaultDispatcherBuilder(DefaultRawParsingContextBuilder builder, ServiceLifetime? lifetime = null)
            {
                _builder = builder;
                _lifetime = lifetime;
            }

            public DefaultDispatcherBuilder WithMessage<TMessage>(Action<ParserServiceBuilder<IDanmakuClient, byte[], IBilibiliRawMessageParser<TMessage>>> parserBuilderAction, Action<SubscriptionServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageSubscription<TMessage>>> subscriptionBuilderAction, ServiceLifetime? lifetime = null) where TMessage : IBilibiliRawMessage
            {
                _builder.WithMessage(parserBuilderAction, dispatcher =>
                {
                    dispatcher.AddDefault(subscriptionBuilderAction, lifetime);
                }, _lifetime);
                return this;
            }

            public readonly struct DefaultSubscriptionBuilder
            {
                private readonly DefaultDispatcherBuilder _builder;

                private readonly ServiceLifetime? _lifetime;

                public DefaultSubscriptionBuilder(DefaultDispatcherBuilder builder, ServiceLifetime? lifetime = null)
                {
                    _builder = builder;
                    _lifetime = lifetime;
                }

                public DefaultSubscriptionBuilder WithMessage<TMessage>(Action<ParserServiceBuilder<IDanmakuClient, byte[], IBilibiliRawMessageParser<TMessage>>> parserBuilderAction, Action<HandlerServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageHandler<TMessage>>> handlerBuilderAction, ServiceLifetime? lifetime = null) where TMessage : IBilibiliRawMessage
                {
                    _builder.WithMessage(parserBuilderAction, subscription =>
                    {
                        subscription.AddDefault(handlerBuilderAction, lifetime);
                    }, _lifetime);
                    return this;
                }
            }
        }
    }

    public readonly struct DefaultJsonParsingContextBuilder
    {
        private readonly ParsingContextServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonParsingContext> _contextBuilder;

        public DefaultJsonParsingContextBuilder(ParsingContextServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonParsingContext> contextBuilder)
        {
            _contextBuilder = contextBuilder;
        }

        public DefaultJsonParsingContextBuilder WithMessage<TMessage>(Action<ParserServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonMessageParser<TMessage>>> parserBuilderAction, Action<MessageDispatcherServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageDispatcher<TMessage>>> dispatcherBuilderAction, ServiceLifetime? lifetime = null) where TMessage : IBilibiliJsonMessage
        {
            var contextBuilder = _contextBuilder;
            var builder = contextBuilder.Builder;

            var parserBuilder = builder.WithParser<IBilibiliJsonMessageParser<TMessage>>();
            parserBuilderAction.Invoke(parserBuilder);

            var dispatcherBuilder = builder.WithMessage<TMessage>().WithDefaultDispatcher();
            dispatcherBuilderAction.Invoke(dispatcherBuilder);

            contextBuilder.AddComponent<BilibiliJsonParsingContext<TMessage>>(lifetime);
            return this;
        }

        public readonly struct DefaultDispatcherBuilder
        {
            private readonly DefaultJsonParsingContextBuilder _builder;

            private readonly ServiceLifetime? _lifetime;

            public DefaultDispatcherBuilder(DefaultJsonParsingContextBuilder builder, ServiceLifetime? lifetime = null)
            {
                _builder = builder;
                _lifetime = lifetime;
            }

            public DefaultDispatcherBuilder WithMessage<TMessage>(Action<ParserServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonMessageParser<TMessage>>> parserBuilderAction, Action<SubscriptionServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageSubscription<TMessage>>> subscriptionBuilderAction, ServiceLifetime? lifetime = null) where TMessage : IBilibiliJsonMessage
            {
                _builder.WithMessage(parserBuilderAction, dispatcher =>
                {
                    dispatcher.AddDefault(subscriptionBuilderAction, lifetime);
                }, _lifetime);
                return this;
            }

            public readonly struct DefaultSubscriptionBuilder
            {
                private readonly DefaultDispatcherBuilder _builder;

                private readonly ServiceLifetime? _lifetime;

                public DefaultSubscriptionBuilder(DefaultDispatcherBuilder builder, ServiceLifetime? lifetime = null)
                {
                    _builder = builder;
                    _lifetime = lifetime;
                }

                public DefaultSubscriptionBuilder AddMessage<TMessage>(Action<ParserServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonMessageParser<TMessage>>> parserBuilderAction, Action<HandlerServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageHandler<TMessage>>> handlerBuilderAction, ServiceLifetime? lifetime = null) where TMessage : IBilibiliJsonMessage
                {
                    _builder.WithMessage(parserBuilderAction, subscription =>
                    {
                        subscription.AddDefault(handlerBuilderAction, lifetime);
                    }, _lifetime);
                    return this;
                }
            }
        }
    }
}
