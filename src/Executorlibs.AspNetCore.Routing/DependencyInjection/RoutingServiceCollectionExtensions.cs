using System.Linq;
using Executorlibs.AspNetCore.Routing.Matching;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Executorlibs.Extensions.DependencyInjection
{
    public static class RoutingServiceCollectionExtensions
    {
        public static IServiceCollection ReplaceAcceptsMatcherPolicy(this IServiceCollection services)
        {
            ServiceDescriptor? acceptsMatcherPolicyDescriptor = services.FirstOrDefault(p => p.ServiceType == typeof(MatcherPolicy) && p.ImplementationType == ModifiedAcceptsMatcherPolicy.AcceptsMatcherPolicyType);
            if (acceptsMatcherPolicyDescriptor != null)
            {
                services.Remove(acceptsMatcherPolicyDescriptor);
                services.TryAddSingleton(ModifiedAcceptsMatcherPolicy.AcceptsMatcherPolicyType);
                services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(MatcherPolicy), typeof(ModifiedAcceptsMatcherPolicy)));
            }
            return services;
        }
    }
}
