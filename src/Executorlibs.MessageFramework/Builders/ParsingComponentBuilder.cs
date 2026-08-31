using Executorlibs.MessageFramework.Clients;

namespace Executorlibs.MessageFramework.Builders
{
    public abstract class ParsingComponentBuilder<TClient, TRawdata, TComponent> : ServiceBuilder<TComponent> where TClient : class, IMessageClient
                                                                                                              where TComponent : class
    {
        protected readonly ParsingServiceBuilder<TClient, TRawdata> _builder;
        
        public ParsingServiceBuilder<TClient, TRawdata> Builder => _builder;

        protected ParsingComponentBuilder(ParsingServiceBuilder<TClient, TRawdata> builder) : base(builder.Services)
        {
            _builder = builder;
        }

        protected ParsingComponentBuilder(ParsingComponentBuilder<TClient, TRawdata, TComponent> builder) : this(builder._builder)
        {

        }
    }

    public abstract class ParsingEnumerableComponentBuilder<TClient, TRawdata, TComponent> : EnumerableServiceBuilder<TComponent> where TClient : class, IMessageClient
                                                                                                                                  where TComponent : class
    {
        protected readonly ParsingServiceBuilder<TClient, TRawdata> _builder;

        public ParsingServiceBuilder<TClient, TRawdata> Builder => _builder;

        protected ParsingEnumerableComponentBuilder(ParsingServiceBuilder<TClient, TRawdata> builder) : base(builder.Services)
        {
            _builder = builder;
        }

        protected ParsingEnumerableComponentBuilder(ParsingEnumerableComponentBuilder<TClient, TRawdata, TComponent> builder) : this(builder._builder)
        {

        }
    }
}
