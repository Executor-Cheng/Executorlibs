using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.AspNetCore.Identity
{
    public interface IUserClaimsPrincipalFactoryProvider<TUser, TContext> where TUser : class where TContext : DbContext
    {
        IUserClaimsPrincipalFactory<TUser> GetFactory();
    }

    public class UserClaimsPrincipalFactoryProvider<TUser, TContext> : IUserClaimsPrincipalFactoryProvider<TUser, TContext> where TUser : class where TContext : DbContext
    {
        protected readonly IServiceProvider _services;

        protected readonly IUserClaimsPrincipalFactoryResolver<TUser> _resolver;

        public UserClaimsPrincipalFactoryProvider(IServiceProvider services, IUserClaimsPrincipalFactoryResolver<TUser> resolver)
        {
            _services = services;
            _resolver = resolver;
        }

        public virtual IUserClaimsPrincipalFactory<TUser> GetFactory()
        {
            return (IUserClaimsPrincipalFactory<TUser>)_services.GetRequiredService(_resolver.ResolveFactoryType(typeof(TContext)));
        }
    }

    public sealed class DefaultUserClaimsPrincipalFactoryProvider<TUser, TContext> : UserClaimsPrincipalFactoryProvider<TUser, TContext> where TUser : class where TContext : DbContext
    {
        public DefaultUserClaimsPrincipalFactoryProvider(IServiceProvider services, IUserClaimsPrincipalFactoryResolver<TUser> resolver) : base(services, resolver)
        {

        }
    }
}
