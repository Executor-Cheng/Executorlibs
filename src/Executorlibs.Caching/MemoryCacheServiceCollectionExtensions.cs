using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up memory cache related services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class MemoryCacheServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a non distributed in memory implementation of <see cref="IMemoryCache"/> to the
        /// <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddMemoryCache<TKey, TValue>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Singleton) where TKey : notnull
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddOptions();
            services.TryAdd(new ServiceDescriptor(typeof(IMemoryCache<TKey, TValue>), typeof(MemoryCache<TKey, TValue>), lifetime));

            return services;
        }

        /// <summary>
        /// Adds a non distributed in memory implementation of <see cref="IMemoryCache"/> to the
        /// <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">
        /// The <see cref="Action{MemoryCacheOptions}"/> to configure the provided <see cref="MemoryCacheOptions{TKey, TValue}"/>.
        /// </param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddMemoryCache<TKey, TValue>(this IServiceCollection services, Action<MemoryCacheOptions<TKey, TValue>> setupAction, ServiceLifetime lifetime = ServiceLifetime.Singleton) where TKey : notnull
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.AddMemoryCache<TKey, TValue>(lifetime);
            services.Configure(setupAction);

            return services;
        }
    }
}
