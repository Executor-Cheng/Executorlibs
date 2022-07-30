using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Identity;

namespace Executorlibs.AspNetCore.Identity
{
    public interface IUserClaimsPrincipalFactoryResolver<TUser> where TUser : class
    {
        Type ResolveFactoryType(Type contextType);
    }

    public class UserClaimsPrincipalFactoryResolver<TUser> : TypeResolver, IUserClaimsPrincipalFactoryResolver<TUser> where TUser : class
    {
        private readonly ConcurrentDictionary<Type, Type> _factoryMapping;

        public UserClaimsPrincipalFactoryResolver()
        {
            _factoryMapping = new ConcurrentDictionary<Type, Type>();
        }

        public Type ResolveFactoryType(Type contextType)
        {
            if (_factoryMapping.TryGetValue(contextType, out Type? factoryType))
            {
                return factoryType;
            }
            return _factoryMapping[contextType] = ResolveFactoryTypeCore(contextType);
        }

        protected virtual Type ResolveFactoryTypeCore(Type contextType)
        {
            Type userType = typeof(TUser);
            if (FindGenericBaseType(userType, typeof(IdentityUser<>)) == null)
            {
                throw new InvalidOperationException($"The type '{userType}' was not derived from IdentityUser<>.");
            }
            return typeof(IUserClaimsPrincipalFactory<,>).MakeGenericType(userType, contextType);
        }
    }
    
    public class UserClaimsPrincipalFactoryResolver<TUser, TRole> : UserClaimsPrincipalFactoryResolver<TUser> where TUser : class where TRole : class
    {
        protected override Type ResolveFactoryTypeCore(Type contextType)
        {
            Type userType = typeof(TUser);
            if (FindGenericBaseType(userType, typeof(IdentityUser<>)) == null)
            {
                throw new InvalidOperationException($"The type '{userType}' was not derived from IdentityUser<>.");
            }
            Type roleType = typeof(TRole);
            if (FindGenericBaseType(roleType, typeof(IdentityRole<>)) == null)
            {
                throw new InvalidOperationException($"The type '{roleType}' was not derived from IdentityRole<>.");
            }
            return typeof(IUserClaimsPrincipalFactory<,,>).MakeGenericType(userType, roleType, contextType);
        }
    }

    public sealed class DefaultUserClaimsPrincipalFactoryResolver<TUser> : UserClaimsPrincipalFactoryResolver<TUser> where TUser : class
    {

    }

    public sealed class DefaultUserClaimsPrincipalFactoryResolver<TUser, TRole> : UserClaimsPrincipalFactoryResolver<TUser, TRole> where TUser : class where TRole : class
    {

    }
}
