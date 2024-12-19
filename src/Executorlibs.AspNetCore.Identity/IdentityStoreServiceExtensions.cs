using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Executorlibs.AspNetCore.Identity
{
    public static class IdentityStoreServiceExtensions
    {
        private static IdentityBuilder AddDefaultEFStoresServices(this IdentityBuilder builder)
        {
            var services = builder.Services;
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
            var services = builder.Services;

            services.TryAddSingleton<IIdentityUserStoreResolver<TUser>, DefaultIdentityStoreResolver<TUser>>();
            services.TryAddSingleton<IUserClaimsPrincipalFactoryResolver<TUser>, DefaultUserClaimsPrincipalFactoryResolver<TUser>>();

            services.TryAddScoped(typeof(UserOnlyStoreProxy<,,>));
            services.TryAddScoped(typeof(UserOnlyStoreProxy<,,,,,>));

            services.TryAddScoped<ManagerProvider<TUser, TContext>, DefaultManagerProvider<TUser, TContext>>();

            static ManagerProvider<TUser, TContext> GetManagerProvider(IServiceProvider services)
            {
                return services.GetRequiredService<ManagerProvider<TUser, TContext>>();
            }
            services.TryAddScoped<IUserManagerProvider<TUser>>(GetManagerProvider);
            services.TryAddScoped<ISignInManagerProvider<TUser>>(GetManagerProvider);

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
            var services = builder.Services;

            services.TryAddSingleton<IdentityStoreResolver<TUser, TRole>, DefaultIdentityStoreResolver<TUser, TRole>>();

            static IdentityStoreResolver<TUser, TRole> GetIdentityStoreResolver(IServiceProvider services)
            {
                return services.GetRequiredService<IdentityStoreResolver<TUser, TRole>>();
            }
            services.TryAddSingleton<IIdentityUserStoreResolver<TUser>>(GetIdentityStoreResolver);
            services.TryAddSingleton<IIdentityRoleStoreResolver<TRole>>(GetIdentityStoreResolver);
            services.TryAddSingleton<IUserClaimsPrincipalFactoryResolver<TUser>, DefaultUserClaimsPrincipalFactoryResolver<TUser, TRole>>();

            services.TryAddScoped(typeof(RoleManager<,>), typeof(DefaultRoleManager<,>));
            services.TryAddScoped(typeof(UserStoreProxy<,,,>));
            services.TryAddScoped(typeof(UserStoreProxy<,,,,,,,,>));
            services.TryAddScoped(typeof(RoleStoreProxy<,,>));
            services.TryAddScoped(typeof(RoleStoreProxy<,,,,>));

            services.TryAddScoped(typeof(IUserClaimsPrincipalFactory<,,>), typeof(DefaultUserClaimsPrincipalFactory<,,>));

            services.TryAddScoped<ManagerProvider<TUser, TRole, TContext>, DefaultManagerProvider<TUser, TRole, TContext>>();

            static ManagerProvider<TUser, TRole, TContext> GetManagerProvider(IServiceProvider services)
            {
                return services.GetRequiredService<ManagerProvider<TUser, TRole, TContext>>();
            }
            services.TryAddScoped<IUserManagerProvider<TUser>>(GetManagerProvider);
            services.TryAddScoped<ISignInManagerProvider<TUser>>(GetManagerProvider);
            services.TryAddScoped<IRoleManagerProvider<TRole>>(GetManagerProvider);

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
