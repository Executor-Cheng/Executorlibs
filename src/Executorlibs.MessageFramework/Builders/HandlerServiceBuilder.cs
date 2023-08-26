using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class HandlerServiceBuilder<TClient, TMessage, THandler> : MessageFrameworkComponentBuilder<TClient, TMessage, THandler>
                                                                      where TClient : class, IMessageClient
                                                                      where TMessage : IMessage
                                                                      where THandler : class, IMessageHandler<TClient, TMessage>
    {
        protected override ServiceLifetime ComponentLifetime => ServiceLifetime.Scoped;

        public HandlerServiceBuilder(MessageFrameworkBuilder<TClient, TMessage> builder) : base(builder, new EnumerableServiceBuilder<THandler>(builder.Services))
        {
            
        }

        public HandlerServiceBuilder(HandlerServiceBuilder<TClient, TMessage, THandler> builder) : base(builder)
        {
            
        }

        //public virtual HandlerServiceBuilder<TClient, TMessage, THandler> AddHandler<THandlerImpl>(ServiceLifetime lifetime = ServiceLifetime.Scoped) where THandlerImpl : class, THandler
        //{
        //    _componentBuilder.AddService<THandlerImpl>(lifetime);
        //    return this;
        //}

        //public virtual HandlerServiceBuilder<TClient, TMessage, THandler> AddHandler(THandler handlerInstance)
        //{
        //    _componentBuilder.AddService(handlerInstance);
        //    return this;
        //}

        //public virtual HandlerServiceBuilder<TClient, TMessage, THandler> AddHandler(Func<IServiceProvider, THandler> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        //{
        //    _componentBuilder.AddService(factory, lifetime);
        //    return this;
        //}
    }
}
