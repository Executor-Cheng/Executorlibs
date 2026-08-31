using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Models.General;

namespace Executorlibs.MessageFramework.Builders
{
    public abstract class MessagingComponentBuilder<TClient, TMessage, TComponent> : ServiceBuilder<TComponent> where TClient : class, IMessageClient
                                                                                                                where TMessage : IMessage
                                                                                                                where TComponent : class
    {
        protected readonly MessageFrameworkBuilder<TClient, TMessage> _builder;

        public MessageFrameworkBuilder<TClient, TMessage> Builder => _builder;

        protected MessagingComponentBuilder(MessageFrameworkBuilder<TClient, TMessage> builder) : base(builder.Services)
        {
            _builder = builder;
        }

        protected MessagingComponentBuilder(MessagingComponentBuilder<TClient, TMessage, TComponent> builder) : this(builder._builder)
        {
            
        }
    }

    public abstract class MessagingEnumerableComponentBuilder<TClient, TMessage, TComponent> : EnumerableServiceBuilder<TComponent> where TClient : class, IMessageClient
                                                                                                                                    where TMessage : IMessage
                                                                                                                                    where TComponent : class
    {
        protected readonly MessageFrameworkBuilder<TClient, TMessage> _builder;

        public MessageFrameworkBuilder<TClient, TMessage> Builder => _builder;

        protected MessagingEnumerableComponentBuilder(MessageFrameworkBuilder<TClient, TMessage> builder) : base(builder.Services)
        {
            _builder = builder;
        }

        protected MessagingEnumerableComponentBuilder(MessagingEnumerableComponentBuilder<TClient, TMessage, TComponent> builder) : this(builder._builder)
        {

        }
    }
}
