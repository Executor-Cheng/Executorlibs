using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.AspNetCore.Identity
{
    public interface IIdentityUserStoreProvider<TUser, TContext> where TUser : class where TContext : DbContext
    {
        IUserStore<TUser, TContext> GetUserStore();
    }

    public class IdentityUserStoreProvider<TUser, TContext> : IIdentityUserStoreProvider<TUser, TContext> where TUser : class where TContext : DbContext
    {
        protected readonly IServiceProvider _services;

        protected readonly IIdentityUserStoreResolver<TUser> _resolver;

        public IdentityUserStoreProvider(IServiceProvider services, IIdentityUserStoreResolver<TUser> resolver)
        {
            _services = services;
            _resolver = resolver;
        }

        public virtual IUserStore<TUser, TContext> GetUserStore()
        {
            return (IUserStore<TUser, TContext>)_services.GetRequiredService(_resolver.ResolveUserStoreType(typeof(TContext)));
        }
    }

    public sealed class DefaultIdentityUserStoreProvider<TUser, TContext> : IdentityUserStoreProvider<TUser, TContext> where TUser : class where TContext : DbContext
    {
        public DefaultIdentityUserStoreProvider(IServiceProvider services, IIdentityUserStoreResolver<TUser> resolver) : base(services, resolver)
        {

        }
    }
}
