using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Executorlibs.MessageFramework.Builders;
using Executorlibs.MessageFramework.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.MessageFramework.Parsing.Context;
using Executorlibs.MessageFramework.Extensions;
using Executorlibs.Bilibili.Protocol.Parsing.Contexts;
using Executorlibs.Bilibili.Protocol.Parsing.Parsers;
using Executorlibs.Bilibili.Protocol.Models.General;
using Microsoft.Extensions.DependencyInjection;
using Executorlibs.Bilibili.Protocol.Dispatchers;

namespace Executorlibs.Bilibili.Protocol.Builders
{
    public class BilibiliMessageFrameworkBuilder : ParsingServiceBuilder<IDanmakuClient, byte[]>
    {
        public BilibiliMessageFrameworkBuilder(IServiceCollection services) : base(services)
        {

        }

        public BilibiliMessageFrameworkBuilder(BilibiliMessageFrameworkBuilder builder) : base(builder)
        {

        }
    }

    public class BilibiliRawParsingContextBuilder
    {
        protected readonly BilibiliMessageFrameworkBuilder _builder;

        protected readonly ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext> _contextBuilder;

        public BilibiliMessageFrameworkBuilder Builder => _builder;

        public BilibiliRawParsingContextBuilder(BilibiliMessageFrameworkBuilder builder) : this(builder, builder.WithParsingContext<IBilibiliRawParsingContext>())
        {

        }

        public BilibiliRawParsingContextBuilder(BilibiliRawParsingContextBuilder builder) : this(builder._builder, builder._contextBuilder)
        {

        }

        public BilibiliRawParsingContextBuilder(BilibiliMessageFrameworkBuilder builder, ParsingContextServiceBuilder<IDanmakuClient, byte[], IBilibiliRawParsingContext> contextBuilder)
        {
            _builder = builder;
            _contextBuilder = contextBuilder;
        }

        public virtual BilibiliRawParsingContextBuilder WithMessage<TMessage>(Action<ParserServiceBuilder<IDanmakuClient, byte[], IBilibiliRawMessageParser<TMessage>>> parserBuilderAction, Action<MessageDispatcherServiceBuilder<IDanmakuClient, TMessage, IBilibiliMessageDispatcher<TMessage>>> dispatcherBuilderAction) where TMessage : IBilibiliRawMessage
        {
            parserBuilderAction.Invoke(_builder.WithParser<IBilibiliRawMessageParser<TMessage>>());
            dispatcherBuilderAction.Invoke(_builder.WithMessage<TMessage>().WithDispatcher<IBilibiliMessageDispatcher<TMessage>>());
            _contextBuilder.AddComponent<BilibiliRawParsingContext<TMessage>>();
            return this;
        }
    }
}
