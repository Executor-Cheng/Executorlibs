using System;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Extensions;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Parsing.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public readonly struct DefaultContextSubscriptionBuilder<TClient, TRawdata> where TClient : class, IMessageClient
    {
        private readonly DefaultContextDispatcherBuilder<TClient, TRawdata> _builder;

        private readonly ServiceLifetime? _lifetime;

        public DefaultContextSubscriptionBuilder(DefaultContextDispatcherBuilder<TClient, TRawdata> builder, ServiceLifetime? lifetime = null)
        {
            _builder = builder;
            _lifetime = lifetime;
        }

        public DefaultContextSubscriptionBuilder<TClient, TRawdata> WithMessage<TMessage>(Action<ParserServiceBuilder<TClient, TRawdata, IMessageParser<TClient, TRawdata, TMessage>>> parserBuilderAction, Action<HandlerServiceBuilder<TClient, TMessage, IMessageHandler<TClient, TMessage>>> handlerBuilderAction, ServiceLifetime? lifetime = null) where TMessage : IMessage<TRawdata>
        {
            _builder.WithMessage(parserBuilderAction, subscription =>
            {
                subscription.AddDefault(handlerBuilderAction, lifetime);
            }, _lifetime);
            return this;
        }
    }
}
