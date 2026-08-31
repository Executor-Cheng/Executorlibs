using System.Text.Json;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Dispatchers;
using Executorlibs.Bilibili.Protocol.Handlers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Executorlibs.Bilibili.Protocol.Parsing.Contexts;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Subscriptions;
using Executorlibs.MessageFramework.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.Bilibili.Protocol.Builders
{
    public sealed class BilibiliRawParsingContextBuilder
    {
        private readonly ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext> _contextBuilder;

        public BilibiliRawParsingContextBuilder(ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext> contextBuilder)
        {
            _contextBuilder = contextBuilder;   
        }

        public BilibiliRawParsingContextBuilder AddMessage<TMessage>(ServiceBuilderAction<IBilibiliRawMessageParser<TMessage>> parserBuilderAction, ServiceBuilderAction<IBilibiliMessageHandler<TMessage>>? handlerBuilderAction = null, ServiceLifetime? lifetime = null) where TMessage : IBilibiliRawMessage
        {
            var builder = _contextBuilder.Builder;

            var parserBuilder = builder.WithParser<IBilibiliRawMessageParser<TMessage>>();
            parserBuilderAction.Invoke(parserBuilder);

            var msgBuilder = builder.WithMessage<TMessage>();
            if (handlerBuilderAction != null)
            {
                var handlerBuilder = msgBuilder.WithHandler<IBilibiliMessageHandler<TMessage>>();
                handlerBuilderAction.Invoke(handlerBuilder);
            }

            var dispatcherBuilder = msgBuilder.WithDispatcher<IBilibiliMessageDispatcher<TMessage>>();
            dispatcherBuilder.AddService<BilibiliMessageDispatcher<TMessage>>(lifetime);

            _contextBuilder.AddService<BilibiliRawParsingContext<TMessage>>(lifetime);
            return this; 
        }
    }

    public sealed class BilibiliJsonParsingContextBuilder
    {
        private readonly ParsingContextServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonParsingContext> _contextBuilder;

        public BilibiliJsonParsingContextBuilder(ParsingContextServiceBuilder<IDanmakuClient, JsonElement, IBilibiliJsonParsingContext> contextBuilder)
        {
            _contextBuilder = contextBuilder;
        }

        public BilibiliJsonParsingContextBuilder AddMessage<TMessage>(ServiceBuilderAction<IBilibiliJsonMessageParser<TMessage>> parserBuilderAction, ServiceBuilderAction<IBilibiliMessageHandler<TMessage>>? handlerBuilderAction = null, ServiceLifetime? lifetime = null) where TMessage : IBilibiliJsonMessage
        {
            var builder = _contextBuilder.Builder;

            var parserBuilder = builder.WithParser<IBilibiliJsonMessageParser<TMessage>>();
            parserBuilderAction.Invoke(parserBuilder);

            var msgBuilder = builder.WithMessage<TMessage>();
            if (handlerBuilderAction != null)
            {
                var handlerBuilder = msgBuilder.WithHandler<IBilibiliMessageHandler<TMessage>>();
                handlerBuilderAction.Invoke(handlerBuilder);
            }

            var dispatcherBuilder = msgBuilder.WithDispatcher<IBilibiliMessageDispatcher<TMessage>>();
            dispatcherBuilder.AddService<BilibiliMessageDispatcher<TMessage>>(lifetime);

            _contextBuilder.AddService<BilibiliJsonParsingContext<TMessage>>(lifetime);
            return this;
        }
    }

    public sealed class BilibiliDispatcherBuilder
    {
        private readonly MessageFrameworkBuilder<IDanmakuClient> _builder;

        public MessageFrameworkBuilder<IDanmakuClient> Builder => _builder;

        public BilibiliDispatcherBuilder(MessageFrameworkBuilder<IDanmakuClient> builder)
        {
            _builder = builder;
        }

        public BilibiliDispatcherBuilder AddMessage<TMessage>(ServiceBuilderAction<IBilibiliMessageHandler<TMessage>>? handlerBuilderAction = null, ServiceLifetime? lifetime = null) where TMessage : IBilibiliMessage
        {
            var builder = _builder.WithMessage<TMessage>();

            if (handlerBuilderAction != null)
            {
                var handlerBuilder = builder.WithHandler<IBilibiliMessageHandler<TMessage>>();
                handlerBuilderAction.Invoke(handlerBuilder);
            }

            var dispatcherBuilder = builder.WithDispatcher<IBilibiliMessageDispatcher<TMessage>>();
            dispatcherBuilder.AddService<BilibiliMessageDispatcher<TMessage>>(lifetime);
            return this;
        }
    }
}
