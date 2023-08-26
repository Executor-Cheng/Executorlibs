using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Executorlibs.MessageFramework.Builders
{
    public abstract class ServiceBuilder
    {
        public IServiceCollection Services { get; }

        public ServiceBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public ServiceBuilder(ServiceBuilder builder) : this(builder.Services)
        {

        }
    }

    public class ServiceBuilder<TService> : ServiceBuilder where TService : class
    {
        public ServiceBuilder(IServiceCollection services) : base(services)
        {

        }

        protected ServiceBuilder(ServiceBuilder<TService> builder) : base(builder)
        {

        }

        public virtual ServiceBuilder<TService> AddService<TImpl>(ServiceLifetime lifetime = ServiceLifetime.Scoped) where TImpl : class, TService
        {
            Services.TryAdd(new ServiceDescriptor(typeof(TService), typeof(TImpl), lifetime));
            return this;
        }

        public virtual ServiceBuilder<TService> AddService<TImpl>(TImpl instance) where TImpl : class, TService
        {
            Services.TryAdd(new ServiceDescriptor(typeof(TService), instance));
            return this;
        }

        public virtual ServiceBuilder<TService> AddService<TImpl>(Func<IServiceProvider, TImpl> factory, ServiceLifetime lifetime = ServiceLifetime.Scoped) where TImpl : class, TService
        {
            Services.TryAdd(new ServiceDescriptor(typeof(TService), factory, lifetime));
            return this;
        }
    }
}
