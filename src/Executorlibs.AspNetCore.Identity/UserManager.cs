using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Executorlibs.AspNetCore.Identity
{
    public class UserManager<TUser, TContext> : UserManager<TUser> where TUser : class where TContext : DbContext
    {
        public UserManager(IUserStore<TUser, TContext> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<TUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }
    }

    public sealed class DefaultUserManager<TUser, TContext> : UserManager<TUser, TContext> where TUser : class where TContext : DbContext
    {
        public DefaultUserManager(IIdentityUserStoreProvider<TUser, TContext> storeProvider, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<TUser> passwordHasher, IEnumerable<IUserValidator<TUser>> userValidators, IEnumerable<IPasswordValidator<TUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<DefaultUserManager<TUser, TContext>> logger)
            : base(storeProvider.GetUserStore(), optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }
    }
}
