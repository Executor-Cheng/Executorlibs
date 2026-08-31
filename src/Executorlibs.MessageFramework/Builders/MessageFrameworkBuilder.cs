using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class MessageFrameworkBuilder<TClient> : ServiceBuilder<TClient> where TClient : class, IMessageClient
    {
        protected override ServiceLifetime DefaultLifetime => ServiceLifetime.Scoped;

        public MessageFrameworkBuilder(IServiceCollection services) : base(services)
        {
            
        }

        public MessageFrameworkBuilder(MessageFrameworkBuilder<TClient> builder) : base(builder.Services)
        {
            
        }

        public virtual MessageFrameworkBuilder<TClient, TMessage> WithMessage<TMessage>() where TMessage : IMessage
        {
            return new MessageFrameworkBuilder<TClient, TMessage>(Services);
        }

        public virtual ParsingServiceBuilder<TClient, TRawdata> WithRawdata<TRawdata>()
        {
            return new ParsingServiceBuilder<TClient, TRawdata>(Services);
        }
    }

    public class MessageFrameworkBuilder<TClient, TMessage> : MessageFrameworkBuilder<TClient> where TClient : class, IMessageClient where TMessage : IMessage
    {
        public MessageFrameworkBuilder(IServiceCollection services) : base(services)
        {

        }

        public MessageFrameworkBuilder(MessageFrameworkBuilder<TClient, TMessage> builder) : base(builder)
        {

        }

        public virtual MessageDispatcherServiceBuilder<TClient, TMessage, TDispatcher> WithDispatcher<TDispatcher>() where TDispatcher : class, IMessageDispatcher<TClient, TMessage>
        {
            return new MessageDispatcherServiceBuilder<TClient, TMessage, TDispatcher>(this);
        }

        public virtual HandlerServiceBuilder<TClient, TMessage, THandler> WithHandler<THandler>() where THandler : class, IMessageHandler<TClient, TMessage>
        {
            return new HandlerServiceBuilder<TClient, TMessage, THandler>(this);
        }
    }
}
