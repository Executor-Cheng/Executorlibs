using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Executorlibs.AspNetCore.Identity
{
    public class RoleManager<TRole, TContext> : RoleManager<TRole> where TRole : class where TContext : DbContext
    {
        public RoleManager(IRoleStore<TRole, TContext> store, IEnumerable<IRoleValidator<TRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<TRole>> logger)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {

        }
    }

    public sealed class DefaultRoleManager<TRole, TContext> : RoleManager<TRole, TContext> where TRole : class where TContext : DbContext
    {
        public DefaultRoleManager(IIdentityRoleStoreProvider<TRole, TContext> storeProvider, IEnumerable<IRoleValidator<TRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<DefaultRoleManager<TRole, TContext>> logger)
            : base(storeProvider.GetRoleStore(), roleValidators, keyNormalizer, errors, logger)
        {

        }
    }
}
