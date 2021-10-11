using System;
using Executorlibs.MessageFramework.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.MessageFramework.Invoking.Attributes
{
    public class RegisterMessageSubscriptionResolverAttribute : RegisterBaseAttribute
    {
        public RegisterMessageSubscriptionResolverAttribute(Type implementationType) : this(implementationType, null)
        {

        }

        public RegisterMessageSubscriptionResolverAttribute(Type implementationType, ServiceLifetime? lifetime) : base(implementationType, lifetime)
        {

        }

        protected override Type GetServiceType(Type implementationType)
        {
            Type openGeneric = typeof(IMessageSubscriptionResolver<,>);
            foreach (Type interfaceType in implementationType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == openGeneric)
                {
                    return interfaceType;
                }
            }
            throw new ArgumentException($"给定的 {implementationType.Name} 不实现 {openGeneric.Name}", nameof(implementationType));
        }
    }
}
