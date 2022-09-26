using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.AspNetCore.Identity
{
    public interface IUserClaimsPrincipalFactory<TUser, TContext> : IUserClaimsPrincipalFactory<TUser> where TUser : class where TContext : DbContext
    {

    }

    public class UserClaimsPrincipalFactory<TUser, TContext> : IUserClaimsPrincipalFactory<TUser, TContext> where TUser : class where TContext : DbContext
    {
        protected readonly IServiceProvider _services;

        protected readonly IDictionary<TUser, IUserClaimsPrincipalLoader<TUser, TContext>> _userLoaders;

        public UserClaimsPrincipalFactory(IServiceProvider services)
        {
            _services = services;
            _userLoaders = new Dictionary<TUser, IUserClaimsPrincipalLoader<TUser, TContext>>();
        }

        protected virtual IUserClaimsPrincipalLoader<TUser, TContext> GetUserLoader(TUser user)
        {
            if (!_userLoaders.TryGetValue(user, out IUserClaimsPrincipalLoader<TUser, TContext>? loader))
            {
                loader = CreateUserLoader(user);
                _userLoaders.Add(user, loader);
            }
            return loader;
        }

        protected virtual IUserClaimsPrincipalLoader<TUser, TContext> CreateUserLoader(TUser user)
        {
            return _services.GetRequiredService<IUserClaimsPrincipalLoader<TUser, TContext>>();
        }

        public virtual Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            IUserClaimsPrincipalLoader<TUser, TContext> loader = GetUserLoader(user);
            return loader.CreateAsync(user);
        }
    }

    public sealed class DefaultUserClaimsPrincipalFactory<TUser, TContext> : UserClaimsPrincipalFactory<TUser, TContext> where TUser : class where TContext : DbContext
    {
        public DefaultUserClaimsPrincipalFactory(IServiceProvider services) : base(services)
        {

        }
    }

    public interface IUserClaimsPrincipalFactory<TUser, TRole, TContext> : IUserClaimsPrincipalFactory<TUser, TContext> where TUser : class where TRole : class where TContext : DbContext
    {

    }

    public class UserClaimsPrincipalFactory<TUser, TRole, TContext> : UserClaimsPrincipalFactory<TUser, TContext>,
                                                                      IUserClaimsPrincipalFactory<TUser, TRole, TContext> where TUser : class where TRole : class where TContext : DbContext
    {
        public UserClaimsPrincipalFactory(IServiceProvider services) : base(services)
        {

        }

        protected override IUserClaimsPrincipalLoader<TUser, TRole, TContext> CreateUserLoader(TUser user)
        {
            return _services.GetRequiredService<IUserClaimsPrincipalLoader<TUser, TRole, TContext>>();
        }
    }

    public sealed class DefaultUserClaimsPrincipalFactory<TUser, TRole, TContext> : UserClaimsPrincipalFactory<TUser, TRole, TContext> where TUser : class where TRole : class where TContext : DbContext
    {
        public DefaultUserClaimsPrincipalFactory(IServiceProvider services) : base(services)
        {

        }
    }
}
