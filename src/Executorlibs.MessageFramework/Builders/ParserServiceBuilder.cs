using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Parsing.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class ParserServiceBuilder<TClient, TRawdata, TParser> : ParsingComponentBuilder<TClient, TRawdata, TParser>
                                                                    where TClient : class, IMessageClient
                                                                    where TParser : class, IMessageParser<TClient, TRawdata>
    {
        protected override ServiceLifetime ComponentLifetime => ServiceLifetime.Singleton;

        public ParserServiceBuilder(ParsingServiceBuilder<TClient, TRawdata> builder) : base(builder, new EnumerableServiceBuilder<TParser>(builder.Services))
        {
            
        }

        public ParserServiceBuilder(ParserServiceBuilder<TClient, TRawdata, TParser> builder) : base(builder)
        {
            
        }

        //public virtual ParserServiceBuilder<TClient, TRawdata, TParser> AddParser<TParserImpl>(ServiceLifetime lifetime = ServiceLifetime.Singleton) where TParserImpl : class, TParser
        //{
        //    _parserBuilder.AddService<TParserImpl>(lifetime);
        //    return this;
        //}

        //public virtual ParserServiceBuilder<TClient, TRawdata, TParser> AddParser(TParser parserInstance)
        //{
        //    _parserBuilder.AddService(parserInstance);
        //    return this;
        //}

        //public virtual ParserServiceBuilder<TClient, TRawdata, TParser> AddParser(Func<IServiceProvider, TParser> factory, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        //{
        //    _parserBuilder.AddService(factory, lifetime);
        //    return this;
        //}
    }
}
