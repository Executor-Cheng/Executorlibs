using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Parsing.Context;
using Executorlibs.MessageFramework.Parsing.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class DefaultParsingContextBuilder<TClient, TRawdata> where TClient : class, IMessageClient
    {
        protected readonly ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>> _contextBuilder;

        public ParsingServiceBuilder<TClient, TRawdata> Builder => _contextBuilder.Builder;

        public DefaultParsingContextBuilder(DefaultParsingContextBuilder<TClient, TRawdata> builder) : this(builder._contextBuilder)
        {

        }

        public DefaultParsingContextBuilder(ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>> contextBuilder)
        {
            _contextBuilder = contextBuilder;
        }

        protected virtual void ConfigureDispatcher<TMessage>(MessageFrameworkBuilder<TClient, TMessage> builder, ServiceLifetime? lifetime) where TMessage : IMessage
        {
            var dispatcherBuilder = builder.WithDispatcher<IMessageDispatcher<TClient, TMessage>>();
            dispatcherBuilder.AddService<DefaultMessageDispatcher<TClient, TMessage>>(lifetime);
        }

        public DefaultParsingContextBuilder<TClient, TRawdata> AddMessage<TMessage>(ServiceBuilderAction<IMessageParser<TRawdata, TMessage>> parserBuilderAction, ServiceBuilderAction<IMessageHandler<TClient, TMessage>>? handlerBuilderAction = null, ServiceLifetime? lifetime = null) where TMessage : IMessage<TRawdata>
        {
            var builder = _contextBuilder.Builder;

            var parserBuilder = builder.WithParser<IMessageParser<TRawdata, TMessage>>();
            parserBuilderAction.Invoke(parserBuilder);

            var msgBuilder = builder.WithMessage<TMessage>();
            if (handlerBuilderAction != null)
            {
                var handlerBuilder = msgBuilder.WithHandler<IMessageHandler<TClient, TMessage>>();
                handlerBuilderAction.Invoke(handlerBuilder);
            }

            ConfigureDispatcher(msgBuilder, lifetime);

            _contextBuilder.AddService<DefaultParsingContext<TClient, TRawdata, TMessage>>(lifetime);
            return this;
        }
    }
}
