using System;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Models.General;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public abstract class MessageFrameworkComponentBuilder<TClient, TMessage, TComponent> where TClient : class, IMessageClient
                                                                                          where TMessage : IMessage
                                                                                          where TComponent : class
    {
        protected readonly MessageFrameworkBuilder<TClient, TMessage> _builder;

        protected readonly ServiceBuilder<TComponent> _componentBuilder;

        protected abstract ServiceLifetime ComponentLifetime { get; }

        public MessageFrameworkBuilder<TClient, TMessage> Builder => _builder;

        protected MessageFrameworkComponentBuilder(MessageFrameworkBuilder<TClient, TMessage> builder, ServiceBuilder<TComponent> componentBuilder)
        {
            _builder = builder;
            _componentBuilder = componentBuilder;
        }

        protected MessageFrameworkComponentBuilder(MessageFrameworkComponentBuilder<TClient, TMessage, TComponent> builder) : this(builder._builder, builder._componentBuilder)
        {
            
        }

        public virtual MessageFrameworkComponentBuilder<TClient, TMessage, TComponent> AddComponent<TComponentImpl>(ServiceLifetime? lifetime = null) where TComponentImpl : class, TComponent
        {
            _componentBuilder.AddService<TComponentImpl>(lifetime ?? ComponentLifetime);
            return this;
        }

        public virtual MessageFrameworkComponentBuilder<TClient, TMessage, TComponent> AddComponent(TComponent componentInstance)
        {
            _componentBuilder.AddService(componentInstance);
            return this;
        }

        public virtual MessageFrameworkComponentBuilder<TClient, TMessage, TComponent> AddComponent(Func<IServiceProvider, TComponent> factory, ServiceLifetime? lifetime = null)
        {
            _componentBuilder.AddService(factory, lifetime ?? ComponentLifetime);
            return this;
        }
    }
}
