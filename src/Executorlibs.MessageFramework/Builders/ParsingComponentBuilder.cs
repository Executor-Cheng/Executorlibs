using System;
using Executorlibs.MessageFramework.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public abstract class ParsingComponentBuilder<TClient, TRawdata, TComponent> where TClient : class, IMessageClient
                                                                                 where TComponent : class
    {
        protected readonly ParsingServiceBuilder<TClient, TRawdata> _builder;

        protected readonly ServiceBuilder<TComponent> _componentBuilder;

        protected abstract ServiceLifetime ComponentLifetime { get; }

        public ParsingServiceBuilder<TClient, TRawdata> Builder => _builder;

        protected ParsingComponentBuilder(ParsingServiceBuilder<TClient, TRawdata> builder, ServiceBuilder<TComponent> componentBuilder)
        {
            _builder = builder;
            _componentBuilder = componentBuilder;
        }

        protected ParsingComponentBuilder(ParsingComponentBuilder<TClient, TRawdata, TComponent> builder) : this(builder._builder, builder._componentBuilder)
        {

        }

        public virtual ParsingComponentBuilder<TClient, TRawdata, TComponent> AddComponent<TComponentImpl>(ServiceLifetime? lifetime = null) where TComponentImpl : class, TComponent
        {
            _componentBuilder.AddService<TComponentImpl>(lifetime ?? ComponentLifetime);
            return this;
        }

        public virtual ParsingComponentBuilder<TClient, TRawdata, TComponent> AddComponent(TComponent handlerInstance)
        {
            _componentBuilder.AddService(handlerInstance);
            return this;
        }

        public virtual ParsingComponentBuilder<TClient, TRawdata, TComponent> AddComponent(Func<IServiceProvider, TComponent> factory, ServiceLifetime? lifetime = null)
        {
            _componentBuilder.AddService(factory, lifetime ?? ComponentLifetime);
            return this;
        }
    }
}
