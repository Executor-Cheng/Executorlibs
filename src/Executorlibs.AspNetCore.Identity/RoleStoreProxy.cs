using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Executorlibs.AspNetCore.Identity
{
    internal sealed class RoleStoreProxy<TRole, TContext, TKey> : RoleStore<TRole, TContext, TKey>, IRoleStore<TRole, TContext>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TContext : DbContext
    {
        public RoleStoreProxy(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
        {

        }
    }

    internal sealed class RoleStoreProxy<TRole, TContext, TKey, TUserRole, TRoleClaim> : RoleStore<TRole, TContext, TKey, TUserRole, TRoleClaim>, IRoleStore<TRole, TContext>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TContext : DbContext
        where TUserRole : IdentityUserRole<TKey>, new()
        where TRoleClaim : IdentityRoleClaim<TKey>, new()
    {
        public RoleStoreProxy(TContext context, IdentityErrorDescriber? describer = null) : base(context, describer)
        {

        }
    }
}
