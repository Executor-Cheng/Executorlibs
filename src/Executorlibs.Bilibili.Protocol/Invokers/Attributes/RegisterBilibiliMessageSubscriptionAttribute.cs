using System;
using System.Linq;
using Executorlibs.MessageFramework.Invoking.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.Bilibili.Protocol.Invokers.Attributes
{
    public class RegisterBilibiliMessageSubscriptionAttribute : RegisterMessageSubscriptionAttribute
    {
        public RegisterBilibiliMessageSubscriptionAttribute(Type implementationType) : base(implementationType)
        {

        }

        public RegisterBilibiliMessageSubscriptionAttribute(Type implementationType, ServiceLifetime? lifetime) : base(implementationType, lifetime)
        {

        }

        protected override Type GetServiceType(Type implementationType)
        {
            Type serviceType = typeof(IBilibiliMessageSubscription),
                 genericServiceType = typeof(IBilibiliMessageSubscription<>);
            Type[] interfaces = implementationType.GetInterfaces();
            if (implementationType.IsGenericTypeDefinition) // impl IBilibiliMessageSubscription<> or IBilibiliMessageSubscription<xxxxx>
            {
                if (interfaces.Any(p => p == genericServiceType || (p.IsGenericType && p.GetGenericTypeDefinition() == genericServiceType)))
                {
                    return genericServiceType;
                }
                throw new InvalidOperationException($"{implementationType} 未实现 {genericServiceType}");
            }
            Type? constructedServiceType = interfaces.FirstOrDefault(p => p.IsGenericType && p.GetGenericTypeDefinition() == genericServiceType);
            if (constructedServiceType != null)
            {
                return constructedServiceType;
            }
            if (serviceType.IsAssignableFrom(implementationType))
            {
                return serviceType;
            }
            throw new InvalidOperationException($"{implementationType} 未实现 {genericServiceType} 或 {serviceType}");
        }
    }
}
