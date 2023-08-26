using System;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Executorlibs.MessageFramework.Extensions;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Parsing.Context;
using Executorlibs.MessageFramework.Parsing.Parsers;

namespace Executorlibs.MessageFramework.Builders
{
    //public class DefaultRawdataDispatcherBuilder<TClient, TRawdata> where TClient : class, IMessageClient
    //{
    //    protected readonly ParsingServiceBuilder<TClient, TRawdata> _builder;

    //    public DefaultRawdataDispatcherBuilder(ParsingServiceBuilder<TClient, TRawdata> builder)
    //    {
    //        _builder = builder;
    //    }

    //    public DefaultRawdataDispatcherBuilder(DefaultRawdataDispatcherBuilder<TClient, TRawdata> builder) : this(builder._builder)
    //    {

    //    }

    //    public DefaultRawdataDispatcherBuilder<TClient, TRawdata> WithMessage(Action<ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>>> contextBuilder) where TMessage : IMessage
    //    {
    //        var builder = _builder.WithRawdata<TRawdata>();
    //        return WithMessage(builder, contextBuilder);
    //    }

    //    public DefaultRawdataDispatcherBuilder<TClient, TRawdata> WithMessage(ParsingServiceBuilder<TClient, TRawdata> builder, Action<ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>>> contextBuilder) where TMessage : IMessage
    //    {
    //        contextBuilder.Invoke(builder.WithDefaultParsingContext());
    //        return WithMessage(builder);
    //    }

    //    protected DefaultRawdataDispatcherBuilder<TClient, TRawdata> WithMessage(ParsingServiceBuilder<TClient, TRawdata> builder) where TMessage : IMessage
    //    {
    //        builder.WithDefaultDispatcher().AddDefaultDispatcher();
    //        return this;
    //    }
    //}

    public class DefaultParsingContextBuilder<TClient, TRawdata> where TClient : class, IMessageClient // , TParserBuider, TContextBuilder
    {
        protected readonly ParsingServiceBuilder<TClient, TRawdata> _builder;

        protected readonly ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>> _contextBuilder;

        public ParsingServiceBuilder<TClient, TRawdata> Builder => _builder;

        public DefaultParsingContextBuilder(ParsingServiceBuilder<TClient, TRawdata> builder) : this(builder, builder.WithDefaultParsingContext())
        {
            
        }

        public DefaultParsingContextBuilder(DefaultParsingContextBuilder<TClient, TRawdata> builder) : this(builder._builder, builder._contextBuilder)
        {

        }

        public DefaultParsingContextBuilder(ParsingServiceBuilder<TClient, TRawdata> builder, ParsingContextServiceBuilder<TClient, TRawdata, IParsingContext<TClient, TRawdata>> contextBuilder)
        {
            _builder = builder;
            _contextBuilder = contextBuilder;
        }

        public DefaultParsingContextBuilder<TClient, TRawdata> WithMessage<TMessage>(Action<ParserServiceBuilder<TClient, TRawdata, IMessageParser<TClient, TRawdata, TMessage>>> parserBuilderAction, Action<MessageDispatcherServiceBuilder<TClient, TMessage, IMessageDispatcher<TClient, TMessage>>> dispatcherBuilderAction) where TMessage : IMessage<TRawdata>
        {
            parserBuilderAction.Invoke(_builder.WithParser<IMessageParser<TClient, TRawdata, TMessage>>());
            dispatcherBuilderAction.Invoke(_builder.WithMessage<TMessage>().WithDefaultDispatcher());
            _contextBuilder.AddComponent<DefaultParsingContext<TClient, TRawdata, TMessage>>();
            return this;
        }
    }
}
