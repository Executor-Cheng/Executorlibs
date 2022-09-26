using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.AspNetCore.Identity
{
    public interface IIdentityRoleStoreProvider<TRole, TContext> where TRole : class where TContext : DbContext
    {
        IRoleStore<TRole, TContext> GetRoleStore();
    }

    public class IdentityRoleStoreProvider<TRole, TContext> : IIdentityRoleStoreProvider<TRole, TContext> where TRole : class where TContext : DbContext
    {
        protected readonly IServiceProvider _services;

        protected readonly IIdentityRoleStoreResolver<TRole> _resolver;

        public IdentityRoleStoreProvider(IServiceProvider services, IIdentityRoleStoreResolver<TRole> resolver)
        {
            _services = services;
            _resolver = resolver;
        }

        public virtual IRoleStore<TRole, TContext> GetRoleStore()
        {
            return (IRoleStore<TRole, TContext>)_services.GetRequiredService(_resolver.ResolveRoleStoreType(typeof(TContext)));
        }
    }

    public sealed class DefaultIdentityRoleStoreProvider<TRole, TContext> : IdentityRoleStoreProvider<TRole, TContext> where TRole : class where TContext : DbContext
    {
        public DefaultIdentityRoleStoreProvider(IServiceProvider services, IIdentityRoleStoreResolver<TRole> resolver) : base(services, resolver)
        {

        }
    }
}
