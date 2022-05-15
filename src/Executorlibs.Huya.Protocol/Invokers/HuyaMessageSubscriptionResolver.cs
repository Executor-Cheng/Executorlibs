using System;
using System.Collections.Generic;
using Executorlibs.Huya.Protocol.Clients;
using Executorlibs.Huya.Protocol.Handlers;
using Executorlibs.MessageFramework.Invoking;

namespace Executorlibs.Huya.Protocol.Invokers
{
    public interface IHuyaMessageSubscriptionResolver : IMessageSubscriptionResolver<IHuyaClient, IHuyaMessageSubscription>
    {

    }

    public class HuyaMessageSubscriptionResolver : MessageSubscriptionResolver<IHuyaClient, IHuyaMessageSubscription>, IHuyaMessageSubscriptionResolver
    {
        public HuyaMessageSubscriptionResolver(IServiceProvider services) : base(services)
        {

        }

        protected override Type GetSubscriptionType(Type messageType)
        {
            return typeof(IHuyaMessageSubscription<>).MakeGenericType(messageType);
        }

        public override IEnumerable<IHuyaMessageSubscription> ResolveByHandler(Type handlerType)
        {
            Type openGeneric = typeof(IHuyaMessageHandler<>);
            List<IHuyaMessageSubscription> subscriptions = new List<IHuyaMessageSubscription>();
            foreach (Type interfaceType in handlerType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == openGeneric)
                {
                    IHuyaMessageSubscription? subscription = ResolveByMessage(interfaceType.GetGenericArguments()[0]);
                    if (subscription != null)
                    {
                        subscriptions.Add(subscription);
                    }
                }
            }
            return subscriptions;
        }
    }
}
