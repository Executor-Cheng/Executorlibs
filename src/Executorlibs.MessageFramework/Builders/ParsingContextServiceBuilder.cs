using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Parsing.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class ParsingContextServiceBuilder<TClient, TRawdata, TParsingContext> : ParsingComponentBuilder<TClient, TRawdata, TParsingContext> where TClient : class, IMessageClient where TParsingContext : class, IParsingContext<TClient, TRawdata>
    {
        protected override ServiceLifetime ComponentLifetime => ServiceLifetime.Scoped;

        public ParsingContextServiceBuilder(ParsingServiceBuilder<TClient, TRawdata> builder) : base(builder, new EnumerableServiceBuilder<TParsingContext>(builder.Services))
        {
            
        }

        public ParsingContextServiceBuilder(ParsingContextServiceBuilder<TClient, TRawdata, TParsingContext> builder) : base(builder)
        {
            
        }

        //public virtual ParsingContextServiceBuilder<TClient, TRawdata, TParsingContext> AddParsingContext<TParsingContextImpl>(ServiceLifetime lifetime = ServiceLifetime.Scoped) where TParsingContextImpl : class, TParsingContext
        //{
        //    _contextBuilder.AddService<TParsingContextImpl>(lifetime);
        //    return this;
        //}

        //public virtual ParsingContextServiceBuilder<TClient, TRawdata, TParsingContext> AddParsingContext(TParsingContext parsingContextInstance)
        //{
        //    _contextBuilder.AddService(parsingContextInstance);
        //    return this;
        //}

        //public virtual ParsingContextServiceBuilder<TClient, TRawdata, TParsingContext> AddParsingContext(Func<IServiceProvider, TParsingContext> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        //{
        //    _contextBuilder.AddService(factory, lifetime);
        //    return this;
        //}
    }
}
