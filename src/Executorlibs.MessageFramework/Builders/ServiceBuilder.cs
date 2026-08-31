using System;
using Executorlibs.MessageFramework.Clients;
using Executorlibs.MessageFramework.Handlers;
using Executorlibs.MessageFramework.Models.General;
using Executorlibs.MessageFramework.Subscriptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Executorlibs.MessageFramework.Builders
{
    public delegate void ServiceBuilderAction<TService>(IServiceBuilder<TService> builder) where TService : class;

    public delegate IServiceBuilder<TTransformed> DependencyBuilderTransformer<TService, TTransformed>(IServiceBuilder<TService> builder) where TService : class where TTransformed : class;

    public abstract class DependencyBuilder<T> where T : class
    {
        public abstract DependencyBuilder<T> Configure(ServiceBuilderAction<T> builder);

        public abstract DependencyBuilder<TTransformed> Transform<TTransformed>(DependencyBuilderTransformer<T, TTransformed> transformer) where TTransformed : class;
    }

    public class DependencyBuilder<T1, T2> where T1 : class where T2 : class
    {
        protected readonly IServiceBuilder<T1> _t1Builder;

        protected readonly IServiceBuilder<T2> _t2Builder;

        public DependencyBuilder(IServiceBuilder<T1> t1Builder, IServiceBuilder<T2> t2Builder)
        {
            _t1Builder = t1Builder;
            _t2Builder = t2Builder;
        }

        public virtual DependencyBuilder<T1, T2> Configure(ServiceBuilderAction<T1> t1Builder, ServiceBuilderAction<T2> t2Builder)
        {
            t1Builder.Invoke(_t1Builder);
            t2Builder.Invoke(_t2Builder);
            return this;
        }

        public virtual DependencyBuilder<T1To, T2To> Transform<T1To, T2To>(ServiceBuilderAction<T1> t1Builder, DependencyBuilderTransformer<T1, T1To> t1Transformer, ServiceBuilderAction<T2> t2Builder, DependencyBuilderTransformer<T2, T2To> t2Transformer) where T1To : class where T2To : class
        {
            return new TransformedDepencencyBuilder<T1To, T2To>(t1Transformer.Invoke(_t1Builder), t2Transformer.Invoke(_t2Builder), () => Configure(t1Builder, t2Builder));
        }
    }

    public sealed class TransformedDepencencyBuilder<T1, T2> : DependencyBuilder<T1, T2> where T1 : class where T2 : class
    {
        private readonly Action _transformAction;

        public TransformedDepencencyBuilder(IServiceBuilder<T1> t1Builder, IServiceBuilder<T2> t2Builder, Action transformAction) : base(t1Builder, t2Builder)
        {
            _transformAction = transformAction;
        }

        public override DependencyBuilder<T1, T2> Configure(ServiceBuilderAction<T1> t1Builder, ServiceBuilderAction<T2> t2Builder)
        {
            base.Configure(t1Builder, t2Builder);
            _transformAction.Invoke();
            return this;
        }
    }

    public abstract class ServiceBuilder
    {
        protected readonly IServiceCollection _services;

        public IServiceCollection Services => _services;

        protected ServiceBuilder(IServiceCollection services)
        {
            _services = services;
        }
    }

    public interface IServiceBuilder<TService> where TService : class
    {
        IServiceCollection Services { get; }

        IServiceBuilder<TService> AddService<TImplementation>(ServiceLifetime? lifetime = null) where TImplementation : class, TService;

        IServiceBuilder<TService> AddService(TService instance);

        IServiceBuilder<TService> AddService<TImplementation>(Func<IServiceProvider, TImplementation> factory, ServiceLifetime? lifetime = null) where TImplementation : class, TService;
    }

    public abstract class ServiceBuilder<TService> : ServiceBuilder, IServiceBuilder<TService> where TService : class
    {
        protected abstract ServiceLifetime DefaultLifetime { get; }

        protected ServiceBuilder(IServiceCollection services) : base(services)
        {

        }

        public virtual IServiceBuilder<TService> AddService<TImplementation>(ServiceLifetime? lifetime = null) where TImplementation : class, TService
        {
            _services.TryAdd(new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime ?? DefaultLifetime));
            return this;
        }

        public virtual IServiceBuilder<TService> AddService(TService instance)
        {
            _services.TryAdd(new ServiceDescriptor(typeof(TService), instance));
            return this;
        }

        public virtual IServiceBuilder<TService> AddService<TImplementation>(Func<IServiceProvider, TImplementation> factory, ServiceLifetime? lifetime = null) where TImplementation : class, TService
        {
            _services.TryAdd(new ServiceDescriptor(typeof(TService), factory, lifetime ?? DefaultLifetime));
            return this;
        }
    }

    public abstract class EnumerableServiceBuilder<TService> : ServiceBuilder<TService> where TService : class
    {
        protected EnumerableServiceBuilder(IServiceCollection services) : base(services)
        {

        }

        public override IServiceBuilder<TService> AddService<TImplementation>(ServiceLifetime? lifetime = null)
        {
            _services.TryAddEnumerable(new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime ?? DefaultLifetime));
            return this;
        }

        public override IServiceBuilder<TService> AddService(TService instance)
        {
            _services.TryAddEnumerable(new ServiceDescriptor(typeof(TService), instance));
            return this;
        }

        public override IServiceBuilder<TService> AddService<TImplementation>(Func<IServiceProvider, TImplementation> factory, ServiceLifetime? lifetime = null)
        {
            _services.TryAddEnumerable(new ServiceDescriptor(typeof(TService), factory, lifetime ?? DefaultLifetime));
            return this;
        }
    }
}
