using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Executorlibs.MessageFramework.Models.General;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class MessageDispatcherServiceBuilder<TClient, TMessage, TDispatcher> : MessageFrameworkComponentBuilder<TClient, TMessage, TDispatcher>
                                                                                   where TClient : class, IMessageClient
                                                                                   where TMessage : IMessage
                                                                                   where TDispatcher : class, IMessageDispatcher<TClient, TMessage>
    {
        protected override ServiceLifetime ComponentLifetime => ServiceLifetime.Scoped;

        public MessageDispatcherServiceBuilder(MessageFrameworkBuilder<TClient, TMessage> builder) : base(builder, new ServiceBuilder<TDispatcher>(builder.Services))
        {
            
        }

        public MessageDispatcherServiceBuilder(MessageDispatcherServiceBuilder<TClient, TMessage, TDispatcher> builder) : base(builder)
        {
            
        }

        //public virtual MessageDispatcherServiceBuilder<TClient, TMessage, TDispatcher> AddDispatcher<TDispatcherImpl>(ServiceLifetime lifetime = ServiceLifetime.Scoped) where TDispatcherImpl : class, TDispatcher
        //{
        //    _componentBuilder.AddService<TDispatcherImpl>(lifetime);
        //    return this;
        //}

        //public virtual MessageDispatcherServiceBuilder<TClient, TMessage, TDispatcher> AddDispatcher(TDispatcher dispatcherInstance)
        //{
        //    _componentBuilder.AddService(dispatcherInstance);
        //    return this;
        //}

        //public virtual MessageDispatcherServiceBuilder<TClient, TMessage, TDispatcher> AddDispatcher(Func<IServiceProvider, TDispatcher> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        //{
        //    _componentBuilder.AddService(factory, lifetime);
        //    return this;
        //}
    }
}
