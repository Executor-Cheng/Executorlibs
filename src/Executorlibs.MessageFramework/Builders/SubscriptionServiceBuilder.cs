using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Subscriptions;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Builders
{
    public class SubscriptionServiceBuilder<TClient, TMessage, TSubscription> : MessageFrameworkComponentBuilder<TClient, TMessage, TSubscription>
                                                                                where TClient : class, IMessageClient
                                                                                where TMessage : IMessage
                                                                                where TSubscription : class, IMessageSubscription<TClient, TMessage>
    {
        protected override ServiceLifetime ComponentLifetime => ServiceLifetime.Scoped;

        public SubscriptionServiceBuilder(MessageFrameworkBuilder<TClient, TMessage> builder) : base(builder, new ServiceBuilder<TSubscription>(builder.Services))
        {
            
        }

        public SubscriptionServiceBuilder(SubscriptionServiceBuilder<TClient, TMessage, TSubscription> builder) : base(builder)
        {
            
        }

        //public virtual SubscriptionServiceBuilder<TClient, TMessage, TSubscription> AddSubscription<TSubscriptionImpl>(ServiceLifetime lifetime = ServiceLifetime.Scoped) where TSubscriptionImpl : class, TSubscription
        //{
        //    _subscriptionBuilder.AddService<TSubscriptionImpl>(lifetime);
        //    return this;
        //}

        //public virtual SubscriptionServiceBuilder<TClient, TMessage, TSubscription> AddSubscription(TSubscription subscriptionInstance)
        //{
        //    _subscriptionBuilder.AddService(subscriptionInstance);
        //    return this;
        //}

        //public virtual SubscriptionServiceBuilder<TClient, TMessage, TSubscription> AddSubscription(Func<IServiceProvider, TSubscription> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        //{
        //    _subscriptionBuilder.AddService(factory, lifetime);
        //    return this;
        //}
    }
}
