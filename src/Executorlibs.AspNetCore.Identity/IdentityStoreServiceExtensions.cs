using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Executorlibs.AspNetCore.Identity
{
    public static class IdentityStoreServiceExtensions
    {
        internal static IdentityBuilder AddDefaultEFStoresServices(this IdentityBuilder builder)
        {
            IServiceCollection services = builder.Services;
            services.TryAddScoped(typeof(UserManager<,>), typeof(DefaultUserManager<,>));
            services.TryAddScoped(typeof(SignInManager<,>), typeof(DefaultSignInManager<,>));
            services.TryAddScoped(typeof(IUserClaimsPrincipalFactory<,>), typeof(DefaultUserClaimsPrincipalFactory<,>));
            services.TryAddTransient(typeof(IIdentityUserStoreProvider<,>), typeof(DefaultIdentityUserStoreProvider<,>));
            services.TryAddTransient(typeof(IUserClaimsPrincipalFactoryProvider<,>), typeof(DefaultUserClaimsPrincipalFactoryProvider<,>));
            return builder;
        }

        public static IdentityBuilder AddDefaultEFStores<TUser, TContext>(this IdentityBuilder builder) where TUser : class where TContext : DbContext
        {
            if (typeof(TUser) != builder.UserType)
            {
                throw new InvalidOperationException();
            }
            IServiceCollection services = builder.Services;

            services.TryAddSingleton<IIdentityUserStoreResolver<TUser>, DefaultIdentityStoreResolver<TUser>>();
            services.TryAddSingleton<IUserClaimsPrincipalFactoryResolver<TUser>, DefaultUserClaimsPrincipalFactoryResolver<TUser>>();

            services.TryAddScoped(typeof(UserOnlyStoreProxy<,,>));
            services.TryAddScoped(typeof(UserOnlyStoreProxy<,,,,,>));

            services.TryAddScoped<ManagerProvider<TUser, TContext>, DefaultManagerProvider<TUser, TContext>>();
            services.TryAddScoped<IUserManagerProvider<TUser>>(services => services.GetRequiredService<ManagerProvider<TUser, TContext>>());
            services.TryAddScoped<ISignInManagerProvider<TUser>>(services => services.GetRequiredService<ManagerProvider<TUser, TContext>>());

            services.TryAddTransient(typeof(IUserClaimsPrincipalLoader<,>), typeof(DefaultUserClaimsPrincipalLoader<,>));

            services.Replace(ServiceDescriptor.Scoped(typeof(UserManager<TUser>), services => services.GetRequiredService<IUserManagerProvider<TUser>>().GetUserManager()));
            services.Replace(ServiceDescriptor.Scoped(typeof(SignInManager<TUser>), services => services.GetRequiredService<ISignInManagerProvider<TUser>>().GetSignInManager()));
            builder.AddDefaultEFStoresServices();
            return builder;
        }

        public static IdentityBuilder AddDefaultEFStores<TUser, TRole, TContext>(this IdentityBuilder builder) where TUser : class where TRole : class where TContext : DbContext
        {
            if (typeof(TUser) != builder.UserType)
            {
                throw new InvalidOperationException();
            }
            if (typeof(TRole) != builder.RoleType)
            {
                throw new InvalidOperationException();
            }
            IServiceCollection services = builder.Services;

            services.TryAddSingleton<IdentityStoreResolver<TUser, TRole>, DefaultIdentityStoreResolver<TUser, TRole>>();
            services.TryAddSingleton<IIdentityUserStoreResolver<TUser>>(services => services.GetRequiredService<IdentityStoreResolver<TUser, TRole>>());
            services.TryAddSingleton<IIdentityRoleStoreResolver<TRole>>(services => services.GetRequiredService<IdentityStoreResolver<TUser, TRole>>());
            services.TryAddSingleton<IUserClaimsPrincipalFactoryResolver<TUser>, DefaultUserClaimsPrincipalFactoryResolver<TUser, TRole>>();

            services.TryAddScoped(typeof(RoleManager<,>), typeof(DefaultRoleManager<,>));
            services.TryAddScoped(typeof(UserStoreProxy<,,,>));
            services.TryAddScoped(typeof(UserStoreProxy<,,,,,,,,>));
            services.TryAddScoped(typeof(RoleStoreProxy<,,>));
            services.TryAddScoped(typeof(RoleStoreProxy<,,,,>));

            services.TryAddScoped(typeof(IUserClaimsPrincipalFactory<,,>), typeof(DefaultUserClaimsPrincipalFactory<,,>));

            services.TryAddScoped<ManagerProvider<TUser, TRole, TContext>, DefaultManagerProvider<TUser, TRole, TContext>>();
            services.TryAddScoped<IUserManagerProvider<TUser>>(services => services.GetRequiredService<ManagerProvider<TUser, TRole, TContext>>());
            services.TryAddScoped<ISignInManagerProvider<TUser>>(services => services.GetRequiredService<ManagerProvider<TUser, TRole, TContext>>());
            services.TryAddScoped<IRoleManagerProvider<TRole>>(services => services.GetRequiredService<ManagerProvider<TUser, TRole, TContext>>());

            services.TryAddTransient(typeof(IUserClaimsPrincipalLoader<,,>), typeof(DefaultUserClaimsPrincipalLoader<,,>));

            services.Replace(ServiceDescriptor.Scoped(typeof(UserManager<TUser>), services => services.GetRequiredService<IUserManagerProvider<TUser>>().GetUserManager()));
            services.Replace(ServiceDescriptor.Scoped(typeof(SignInManager<TUser>), services => services.GetRequiredService<ISignInManagerProvider<TUser>>().GetSignInManager()));
            services.Replace(ServiceDescriptor.Scoped(typeof(RoleManager<TUser>), services => services.GetRequiredService<IRoleManagerProvider<TUser>>().GetRoleManager()));

            services.TryAddTransient(typeof(IIdentityRoleStoreProvider<,>), typeof(DefaultIdentityRoleStoreProvider<,>));

            builder.AddDefaultEFStoresServices();
            return builder;
        }
    }
}
