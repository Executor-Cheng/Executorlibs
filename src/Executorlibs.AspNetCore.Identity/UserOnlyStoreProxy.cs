using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
namespace Executorlibs.AspNetCore.Identity
{
    internal sealed class UserOnlyStoreProxy<TUser, TContext, TKey> : UserOnlyStore<TUser, TContext, TKey>, IUserStore<TUser, TContext>
        where TUser : IdentityUser<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
    {
        public UserOnlyStoreProxy(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
        {

        }
    }

    internal sealed class UserOnlyStoreProxy<TUser, TContext, TKey, TUserClaim, TUserLogin, TUserToken> : UserOnlyStore<TUser, TContext, TKey, TUserClaim, TUserLogin, TUserToken>, IUserStore<TUser, TContext>
        where TUser : IdentityUser<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserToken : IdentityUserToken<TKey>, new()
    {
        public UserOnlyStoreProxy(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
        {

        }
    }
}
