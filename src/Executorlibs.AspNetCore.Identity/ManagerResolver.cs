using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Executorlibs.AspNetCore.Identity
{
    public interface IUserManagerProvider<TUser> where TUser : class
    {
        UserManager<TUser> GetUserManager();
    }

    public interface ISignInManagerProvider<TUser> where TUser : class
    {
        SignInManager<TUser> GetSignInManager();
    }

    public interface IRoleManagerProvider<TRole> where TRole : class
    {
        RoleManager<TRole> GetRoleManager();
    }

    public class ManagerProvider<TUser, TContext> : TypeResolver,
                                                    IUserManagerProvider<TUser>,
                                                    ISignInManagerProvider<TUser> where TUser : class where TContext : DbContext
    {
        protected readonly IServiceProvider _services;

        protected Type _identityDbContextType;

        public ManagerProvider(IServiceProvider services, IHttpContextAccessor accessor)
        {
            _services = services;
            _identityDbContextType = GetIdentityDbContextType(accessor.HttpContext);
        }

        public SignInManager<TUser> GetSignInManager()
        {
            return (SignInManager<TUser>)_services.GetRequiredService(GetSignInManagerType());
        }

        public UserManager<TUser> GetUserManager()
        {
            return (UserManager<TUser>)_services.GetRequiredService(GetUserManagerType());
        }

        protected virtual Type GetSignInManagerType()
        {
            return typeof(SignInManager<,>).MakeGenericType(typeof(TUser), _identityDbContextType);
        }

        protected virtual Type GetUserManagerType()
        {
            return typeof(UserManager<,>).MakeGenericType(typeof(TUser), _identityDbContextType);
        }

        protected virtual Type GetIdentityDbContextType(HttpContext? context)
        {
            if (context != null &&
                context.GetEndpoint() is Endpoint endpoint &&
                endpoint.Metadata.GetMetadata<IdentityDbContextAttribute>() is IdentityDbContextAttribute identityDbContextAttribute)
            {
                return identityDbContextAttribute.DbContextType;
            }
            return typeof(TContext);
        }
    }

    public sealed class DefaultManagerProvider<TUser, TContext> : ManagerProvider<TUser, TContext> where TUser : class where TContext : DbContext
    {
        public DefaultManagerProvider(IServiceProvider services, IHttpContextAccessor accessor) : base(services, accessor)
        {

        }
    }

    public class ManagerProvider<TUser, TRole, TContext> : ManagerProvider<TUser, TContext>,
                                                           IRoleManagerProvider<TRole> where TUser : class where TRole : class where TContext : DbContext
    {
        public ManagerProvider(IServiceProvider services, IHttpContextAccessor accessor) : base(services, accessor)
        {

        }

        public RoleManager<TRole> GetRoleManager()
        {
            return (RoleManager<TRole>)_services.GetRequiredService(GetRoleManagerType());
        }

        protected virtual Type GetRoleManagerType()
        {
            return typeof(RoleManager<,>).MakeGenericType(typeof(TRole), _identityDbContextType);
        }
    }

    public sealed class DefaultManagerProvider<TUser, TRole, TContext> : ManagerProvider<TUser, TRole, TContext> where TUser : class where TRole : class where TContext : DbContext
    {
        public DefaultManagerProvider(IServiceProvider services, IHttpContextAccessor accessor) : base(services, accessor)
        {

        }
    }
}
