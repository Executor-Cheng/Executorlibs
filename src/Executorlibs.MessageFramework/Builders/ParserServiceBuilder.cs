using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Parsing.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class ParserServiceBuilder<TClient, TRawdata, TParser> : ParsingEnumerableComponentBuilder<TClient, TRawdata, TParser>
                                                                    where TClient : class, IMessageClient
                                                                    where TParser : class, IMessageParser<TRawdata>
    {
        protected override ServiceLifetime DefaultLifetime => ServiceLifetime.Singleton;

        public ParserServiceBuilder(ParsingServiceBuilder<TClient, TRawdata> builder) : base(builder)
        {
            
        }

        public ParserServiceBuilder(ParserServiceBuilder<TClient, TRawdata, TParser> builder) : base(builder)
        {
            
        }
    }
}
