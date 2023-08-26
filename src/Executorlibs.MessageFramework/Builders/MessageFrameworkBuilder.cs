using System;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Dispatchers;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Subscriptions;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class MessageFrameworkBuilder<TClient> : ServiceBuilder where TClient : class, IMessageClient
    {
        protected readonly ServiceBuilder<TClient> _clientBuilder;

        public MessageFrameworkBuilder(IServiceCollection services) : base(services)
        {
            _clientBuilder = new ServiceBuilder<TClient>(services);
        }

        public MessageFrameworkBuilder(MessageFrameworkBuilder<TClient> builder) : base(builder)
        {
            _clientBuilder = builder._clientBuilder;
        }

        public virtual MessageFrameworkBuilder<TClient, TMessage> WithMessage<TMessage>() where TMessage : IMessage
        {
            return new MessageFrameworkBuilder<TClient, TMessage>(Services);
        }

        public virtual ParsingServiceBuilder<TClient, TRawdata> WithRawdata<TRawdata>()
        {
            return new ParsingServiceBuilder<TClient, TRawdata>(Services);
        }

        public virtual MessageFrameworkBuilder<TClient> AddClient<TClientImpl>(ServiceLifetime lifetime = ServiceLifetime.Scoped) where TClientImpl : class, TClient
        {
            _clientBuilder.AddService<TClientImpl>(lifetime);
            return this;
        }

        public virtual MessageFrameworkBuilder<TClient> AddClient(TClient clientInstance)
        {
            _clientBuilder.AddService(clientInstance);
            return this;
        }

        public virtual MessageFrameworkBuilder<TClient> AddClient(Func<IServiceProvider, TClient> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            _clientBuilder.AddService(factory, lifetime);
            return this;
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

        public virtual SubscriptionServiceBuilder<TClient, TMessage, TSubscription> WithSubscription<TSubscription>() where TSubscription : class, IMessageSubscription<TClient, TMessage>
        {
            return new SubscriptionServiceBuilder<TClient, TMessage, TSubscription>(this);
        }
    }
}
