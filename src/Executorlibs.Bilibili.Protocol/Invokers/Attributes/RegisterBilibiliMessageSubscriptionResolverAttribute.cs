using System;
using Executorlibs.MessageFramework.Invoking.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.Bilibili.Protocol.Invokers.Attributes
{
    public class RegisterBilibiliMessageSubscriptionResolverAttribute : RegisterMessageSubscriptionResolverAttribute
    {
        public RegisterBilibiliMessageSubscriptionResolverAttribute(Type implementationType) : this(implementationType, null)
        {

        }

        public RegisterBilibiliMessageSubscriptionResolverAttribute(Type implementationType, ServiceLifetime? lifetime) : base(implementationType, lifetime)
        {

        }

        protected override Type GetServiceType(Type implementationType)
        {
            var interfaceType = typeof(IBilibiliMessageSubscriptionResolver);
            if (interfaceType.IsAssignableFrom(implementationType))
            {
                return interfaceType;
            }
            throw new ArgumentException($"给定的 {implementationType.Name} 不实现 {interfaceType.Name}", nameof(implementationType));
        }
    }
}
