using System;
using System.Collections.Generic;
using Executorlibs.Bilibili.Protocol.Clients;
using Executorlibs.Bilibili.Protocol.Handlers;
using Executorlibs.MessageFramework.Invoking;

namespace Executorlibs.Bilibili.Protocol.Invokers
{
    public interface IBilibiliMessageSubscriptionResolver : IMessageSubscriptionResolver<IDanmakuClient, IBilibiliMessageSubscription>
    {

    }

    public class BilibiliMessageSubscriptionResolver : MessageSubscriptionResolver<IDanmakuClient, IBilibiliMessageSubscription>, IBilibiliMessageSubscriptionResolver
    {
        public BilibiliMessageSubscriptionResolver(IServiceProvider services) : base(services)
        {

        }

        protected override Type GetSubscriptionType(Type messageType)
        {
            return typeof(IBilibiliMessageSubscription<>).MakeGenericType(messageType);
        }

        public override IEnumerable<IBilibiliMessageSubscription> ResolveByHandler(Type handlerType)
        {
            Type openGeneric = typeof(IBilibiliMessageHandler<>);
            List<IBilibiliMessageSubscription> subscriptions = new List<IBilibiliMessageSubscription>();
            foreach (Type interfaceType in handlerType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == openGeneric)
                {
                    IBilibiliMessageSubscription? subscription = ResolveByMessage(interfaceType.GetGenericArguments()[0]);
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
