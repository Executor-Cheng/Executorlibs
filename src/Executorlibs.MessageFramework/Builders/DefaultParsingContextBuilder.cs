using System;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Executorlibs.MessageFramework.Extensions;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Parsing.Context;
using Executorlibs.MessageFramework.Parsing.Parsers;
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
    }
}
