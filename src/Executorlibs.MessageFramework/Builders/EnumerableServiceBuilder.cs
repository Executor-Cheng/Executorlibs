using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Executorlibs.MessageFramework.Builders
{
    public class EnumerableServiceBuilder<TService> : ServiceBuilder<TService> where TService : class
    {
        public EnumerableServiceBuilder(IServiceCollection services) : base(services)
        {

        }

        protected EnumerableServiceBuilder(EnumerableServiceBuilder<TService> builder) : base(builder)
        {

        }

        public override ServiceBuilder<TService> AddService<TImpl>(ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Services.TryAddEnumerable(new ServiceDescriptor(typeof(TService), typeof(TImpl), lifetime));
            return this;
        }

        public override ServiceBuilder<TService> AddService<TImpl>(TImpl instance)
        {
            Services.TryAddEnumerable(new ServiceDescriptor(typeof(TService), instance));
            return this;
        }

        public override ServiceBuilder<TService> AddService<TImpl>(Func<IServiceProvider, TImpl> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            Services.TryAddEnumerable(new ServiceDescriptor(typeof(TService), factory, lifetime));
            return this;
        }
    }
}
