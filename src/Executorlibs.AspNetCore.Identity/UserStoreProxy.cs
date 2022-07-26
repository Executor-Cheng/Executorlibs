using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Executorlibs.AspNetCore.Identity
{
    internal sealed class UserStoreProxy<TUser, TRole, TContext, TKey> : UserStore<TUser, TRole, TContext, TKey>, IUserStore<TUser, TContext>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
    {
        public UserStoreProxy(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
        {

        }
    }

    internal sealed class UserStoreProxy<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim> : UserStore<TUser, TRole, TContext, TKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>, IUserStore<TUser, TContext>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserRole : IdentityUserRole<TKey>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserToken : IdentityUserToken<TKey>, new()
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
    {
        public UserStoreProxy(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
        {

        }
    }
}
