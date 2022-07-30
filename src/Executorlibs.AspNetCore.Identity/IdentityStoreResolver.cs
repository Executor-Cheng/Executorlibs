using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Executorlibs.AspNetCore.Identity
{
    public interface IIdentityUserStoreResolver<TUser> where TUser : class
    {
        Type ResolveUserStoreType(Type contextType);
    }

    public interface IIdentityRoleStoreResolver<TRole> where TRole : class
    {
        Type ResolveRoleStoreType(Type contextType);
    }

    public class IdentityStoreResolver<TUser> : TypeResolver, IIdentityUserStoreResolver<TUser> where TUser : class
    {
        protected readonly ConcurrentDictionary<Type, Type> _userStoreMapping;

        public IdentityStoreResolver()
        {
            _userStoreMapping = new ConcurrentDictionary<Type, Type>();
        }

        public virtual Type ResolveUserStoreType(Type contextType)
        {
            if (_userStoreMapping.TryGetValue(contextType, out Type? userStoreType))
            {
                return userStoreType;
            }
            Resolve(contextType);
            return _userStoreMapping[contextType];
        }

        public virtual Type ResolveRoleStoreType(Type contextType)
        {
            throw new NotSupportedException();
        }

        protected virtual void Resolve(Type contextType)
        {
            Type userType = typeof(TUser);
            Type? identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));
            if (identityUserType == null)
            {
                throw new InvalidOperationException($"The type '{userType}' was not derived from IdentityUser<>.");
            }
            Resolve(contextType, FindGenericBaseType(contextType, typeof(IdentityUserContext<,,,,>)), identityUserType.GenericTypeArguments[0]);
        }

        protected virtual void Resolve(Type contextType, Type? identityContextType, Type keyType)
        {
            Type userType = typeof(TUser);
            if (identityContextType == null)
            {
                _userStoreMapping[contextType] = typeof(UserOnlyStoreProxy<,,>).MakeGenericType(userType, contextType, keyType);
                return;
            }
            _userStoreMapping[contextType] = typeof(UserOnlyStoreProxy<,,,,,>).MakeGenericType(userType, contextType,
                    identityContextType.GenericTypeArguments[1],
                    identityContextType.GenericTypeArguments[2],
                    identityContextType.GenericTypeArguments[3],
                    identityContextType.GenericTypeArguments[4]);
        }
    }

    public sealed class DefaultIdentityStoreResolver<TUser> : IdentityStoreResolver<TUser> where TUser : class
    {

    }

    public class IdentityStoreResolver<TUser, TRole> : IdentityStoreResolver<TUser>, IIdentityRoleStoreResolver<TRole> where TUser : class where TRole : class
    {
        private readonly ConcurrentDictionary<Type, Type> _roleStoreMapping;

        public IdentityStoreResolver()
        {
            _roleStoreMapping = new ConcurrentDictionary<Type, Type>();
        }

        public override Type ResolveRoleStoreType(Type contextType)
        {
            if (_roleStoreMapping.TryGetValue(contextType, out Type? roleStoreType))
            {
                return roleStoreType;
            }
            Resolve(contextType);
            return _roleStoreMapping[contextType];
        }

        protected override void Resolve(Type contextType)
        {
            Type userType = typeof(TUser);
            Type? identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));
            if (identityUserType == null)
            {
                throw new InvalidOperationException($"The type '{userType}' was not derived from IdentityUser<>.");
            }
            Type roleType = typeof(TRole);
            if (FindGenericBaseType(roleType, typeof(IdentityRole<>)) == null)
            {
                throw new InvalidOperationException($"The type '{roleType}' was not derived from IdentityRole<>.");
            }
            Resolve(contextType, FindGenericBaseType(contextType, typeof(IdentityDbContext<,,,,,,,>)), identityUserType.GenericTypeArguments[0]);
        }

        protected override void Resolve(Type contextType, Type? identityContextType, Type keyType)
        {
            Type userType = typeof(TUser);
            Type roleType = typeof(TRole);
            if (identityContextType == null)
            {
                _userStoreMapping[contextType] = typeof(UserStoreProxy<,,,>).MakeGenericType(userType, roleType, contextType, keyType);
                _roleStoreMapping[contextType] = typeof(RoleStoreProxy<,,>).MakeGenericType(roleType, contextType, keyType);
                return;
            }
            _userStoreMapping[contextType] = typeof(UserStoreProxy<,,,,,,,,>).MakeGenericType(userType, roleType, contextType,
                    identityContextType.GenericTypeArguments[2],
                    identityContextType.GenericTypeArguments[3],
                    identityContextType.GenericTypeArguments[4],
                    identityContextType.GenericTypeArguments[5],
                    identityContextType.GenericTypeArguments[7],
                    identityContextType.GenericTypeArguments[6]);
            _roleStoreMapping[contextType] = typeof(RoleStoreProxy<,,,,>).MakeGenericType(roleType, contextType,
                    identityContextType.GenericTypeArguments[2],
                    identityContextType.GenericTypeArguments[4],
                    identityContextType.GenericTypeArguments[6]);
        }
    }

    public sealed class DefaultIdentityStoreResolver<TUser, TRole> : IdentityStoreResolver<TUser, TRole> where TUser : class where TRole : class
    {

    }
}
