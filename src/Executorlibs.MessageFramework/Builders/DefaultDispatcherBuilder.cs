using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class DefaultDispatcherBuilder<TClient> where TClient : class, IMessageClient
    {
        protected readonly MessageFrameworkBuilder<TClient> _builder;

        public MessageFrameworkBuilder<TClient> Builder => _builder;

        public DefaultDispatcherBuilder(DefaultDispatcherBuilder<TClient> builder) : this(builder._builder)
        {

        }

        public DefaultDispatcherBuilder(MessageFrameworkBuilder<TClient> builder)
        {
            _builder = builder;
        }

        protected virtual void ConfigureDispatcher<TMessage>(MessageFrameworkBuilder<TClient, TMessage> builder, ServiceLifetime? lifetime) where TMessage : IMessage
        {
            var dispatcherBuilder = builder.WithDispatcher<IMessageDispatcher<TClient, TMessage>>();
            dispatcherBuilder.AddService<DefaultMessageDispatcher<TClient, TMessage>>(lifetime);
        }

        public virtual DefaultDispatcherBuilder<TClient> AddMessage<TMessage>(ServiceBuilderAction<IMessageHandler<TClient, TMessage>>? handlerBuilderAction = null, ServiceLifetime? lifetime = null) where TMessage : IMessage
        {
            var builder = _builder.WithMessage<TMessage>();

            if (handlerBuilderAction != null)
            {
                var handlerBuilder = builder.WithHandler<IMessageHandler<TClient, TMessage>>();
                handlerBuilderAction.Invoke(handlerBuilder);
            }

            ConfigureDispatcher(builder, lifetime);
            return this;
        }
    }
}
