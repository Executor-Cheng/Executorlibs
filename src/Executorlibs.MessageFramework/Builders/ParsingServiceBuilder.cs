using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Executorlibs.MessageFramework.Parsing.Context;
using Executorlibs.MessageFramework.Parsing.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class ParsingServiceBuilder<TClient, TRawdata> : MessageFrameworkBuilder<TClient> where TClient : class, IMessageClient
    {
        public ParsingServiceBuilder(IServiceCollection services) : base(services)
        {
            
        }

        public ParsingServiceBuilder(ParsingServiceBuilder<TClient, TRawdata> builder) : base(builder)
        {
            
        }

        public virtual ParserServiceBuilder<TClient, TRawdata, TParser> WithParser<TParser>() where TParser : class, IMessageParser<TClient, TRawdata>
        {
            return new ParserServiceBuilder<TClient, TRawdata, TParser>(this);
        }

        public virtual RawdataDispatcherServiceBuilder<TClient, TRawdata, TDispatcher> WithDispatcher<TDispatcher>() where TDispatcher : class, IRawdataDispatcher<TClient, TRawdata>
        {
            return new RawdataDispatcherServiceBuilder<TClient, TRawdata, TDispatcher>(this);
        }

        public virtual ParsingContextServiceBuilder<TClient, TRawdata, TParsingContext> WithParsingContext<TParsingContext>() where TParsingContext : class, IParsingContext<TClient, TRawdata>
        {
            return new ParsingContextServiceBuilder<TClient, TRawdata, TParsingContext>(this);
        }
    }

    public readonly struct DependencyBuilder<TService, TImplementation> where TService : class
                                                                        where TImplementation : class, TService
    {
        private readonly ServiceBuilder<TService> _builder;

        private readonly ServiceLifetime _lifetime;

        public DependencyBuilder(ServiceBuilder<TService> builder, ServiceLifetime lifetime)
        {
            _builder = builder;
            _lifetime = lifetime;
        }
    }
}
