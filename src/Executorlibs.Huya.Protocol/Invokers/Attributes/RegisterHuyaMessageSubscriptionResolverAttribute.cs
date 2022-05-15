using System;
using Executorlibs.MessageFramework.Invoking.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.Huya.Protocol.Invokers.Attributes
{
    public class RegisterHuyaMessageSubscriptionResolverAttribute : RegisterMessageSubscriptionResolverAttribute
    {
        public RegisterHuyaMessageSubscriptionResolverAttribute(Type implementationType) : this(implementationType, null)
        {

        }

        public RegisterHuyaMessageSubscriptionResolverAttribute(Type implementationType, ServiceLifetime? lifetime) : base(implementationType, lifetime)
        {

        }

        protected override Type GetServiceType(Type implementationType)
        {
            var interfaceType = typeof(IHuyaMessageSubscriptionResolver);
            if (interfaceType.IsAssignableFrom(implementationType))
            {
                return interfaceType;
            }
            throw new ArgumentException($"给定的 {implementationType.Name} 不实现 {interfaceType.Name}", nameof(implementationType));
        }
    }
}
