using System;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Extensions;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Parsing.Parsers;
using Executorlibs.MessageFramework.Subscriptions;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public readonly struct DefaultContextDispatcherBuilder<TClient, TRawdata> where TClient : class, IMessageClient
    {
        private readonly DefaultParsingContextBuilder<TClient, TRawdata> _builder;

        private readonly ServiceLifetime? _lifetime;

        public DefaultContextDispatcherBuilder(DefaultParsingContextBuilder<TClient, TRawdata> builder, ServiceLifetime? lifetime = null)
        {
            _builder = builder;
            _lifetime = lifetime;
        }

        public DefaultContextDispatcherBuilder<TClient, TRawdata> WithMessage<TMessage>(Action<ParserServiceBuilder<TClient, TRawdata, IMessageParser<TClient, TRawdata, TMessage>>> parserBuilderAction, Action<SubscriptionServiceBuilder<TClient, TMessage, IMessageSubscription<TClient, TMessage>>> subscriptionBuilderAction, ServiceLifetime? lifetime = null) where TMessage : IMessage<TRawdata>
        {
            _builder.WithMessage(parserBuilderAction, dispatcher =>
            {
                dispatcher.AddDefault(subscriptionBuilderAction, lifetime);
            }, _lifetime);
            return this;
        }
    }
}
