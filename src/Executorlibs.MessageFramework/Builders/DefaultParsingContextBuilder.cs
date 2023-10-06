using System;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Executorlibs.MessageFramework.Extensions;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Parsing.Context;
using Executorlibs.MessageFramework.Parsing.Parsers;
using Executorlibs.MessageFramework.Subscriptions;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public readonly struct DefaultParsingContextBuilder<TClient, TRawdata> where TClient : class, IMessageClient
    {
        private readonly ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>> _contextBuilder;

        public DefaultParsingContextBuilder(ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>> contextBuilder)
        {
            _contextBuilder = contextBuilder;
        }

        public DefaultParsingContextBuilder<TClient, TRawdata> WithMessage<TMessage>(Action<ParserServiceBuilder<TClient, TRawdata, IMessageParser<TClient, TRawdata, TMessage>>> parserBuilderAction, Action<MessageDispatcherServiceBuilder<TClient, TMessage, IMessageDispatcher<TClient, TMessage>>> dispatcherBuilderAction, ServiceLifetime? lifetime = null) where TMessage : IMessage<TRawdata>
        {
            var contextBuilder = _contextBuilder;
            var builder = contextBuilder.Builder;

            var parserBuilder = builder.WithParser<IMessageParser<TClient, TRawdata, TMessage>>();
            parserBuilderAction.Invoke(parserBuilder);

            var dispatcherBuilder = builder.WithMessage<TMessage>().WithDefaultDispatcher();
            dispatcherBuilderAction.Invoke(dispatcherBuilder);

            contextBuilder.AddComponent<DefaultParsingContext<TClient, TRawdata, TMessage>>(lifetime);
            return this;
        }

        public readonly struct DefaultDispatcherBuilder
        {
            private readonly DefaultParsingContextBuilder<TClient, TRawdata> _builder;

            private readonly ServiceLifetime? _lifetime;

            public DefaultDispatcherBuilder(DefaultParsingContextBuilder<TClient, TRawdata> builder, ServiceLifetime? lifetime = null)
            {
                _builder = builder;
                _lifetime = lifetime;
            }

            public DefaultDispatcherBuilder WithMessage<TMessage>(Action<ParserServiceBuilder<TClient, TRawdata, IMessageParser<TClient, TRawdata, TMessage>>> parserBuilderAction, Action<SubscriptionServiceBuilder<TClient, TMessage, IMessageSubscription<TClient, TMessage>>> subscriptionBuilderAction, ServiceLifetime? lifetime = null) where TMessage : IMessage<TRawdata>
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

                public DefaultSubscriptionBuilder(DefaultParsingContextBuilder<TClient, TRawdata>.DefaultDispatcherBuilder builder, ServiceLifetime? lifetime = null)
                {
                    _builder = builder;
                    _lifetime = lifetime;
                }

                public DefaultSubscriptionBuilder WithMessage<TMessage>(Action<ParserServiceBuilder<TClient, TRawdata, IMessageParser<TClient, TRawdata, TMessage>>> parserBuilderAction, Action<HandlerServiceBuilder<TClient, TMessage, IMessageHandler<TClient, TMessage>>> handlerBuilderAction, ServiceLifetime? lifetime = null) where TMessage : IMessage<TRawdata>
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
