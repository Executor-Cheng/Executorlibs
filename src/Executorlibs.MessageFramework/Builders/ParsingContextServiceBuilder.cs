using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Parsing.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class ParsingContextServiceBuilder<TClient, TRawdata, TParsingContext> : ParsingEnumerableComponentBuilder<TClient, TRawdata, TParsingContext> where TClient : class, IMessageClient where TParsingContext : class, IParsingContext<TClient, TRawdata>
    {
        protected override ServiceLifetime DefaultLifetime => ServiceLifetime.Scoped;

        public ParsingContextServiceBuilder(ParsingServiceBuilder<TClient, TRawdata> builder) : base(builder)
        {
            
        }

        public ParsingContextServiceBuilder(ParsingContextServiceBuilder<TClient, TRawdata, TParsingContext> builder) : base(builder)
        {
            
        }
    }
}
