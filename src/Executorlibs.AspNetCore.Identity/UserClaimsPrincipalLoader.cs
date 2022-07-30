using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Executorlibs.AspNetCore.Identity
{
    public interface IUserClaimsPrincipalLoader<TUser, TRole, TContext> : IUserClaimsPrincipalLoader<TUser, TContext> where TUser : class where TRole : class where TContext : DbContext
    {
        RoleManager<TRole> RoleManager { get; }

        bool UserRolesLoaded { get; }

        bool RoleClaimsLoaded { get; }

        Task LoadUserRolesAsync();

        Task LoadRoleClaimsAsync();
    }

    public class UserClaimsPrincipalLoader<TUser, TContext> : IUserClaimsPrincipalLoader<TUser, TContext> where TUser : class where TContext : DbContext
    {
        protected readonly ClaimsLoadingOptions _claimsOptions;

        protected virtual ClaimsLoadingOptions ClaimsOptions => _claimsOptions;

        public TUser? User { get; protected set; }

        protected ClaimsIdentity? UserClaims { get; set; }

        public UserManager<TUser> UserManager { get; private set; }

        public IdentityOptions Options { get; private set; }

        public bool UserIdLoaded { get; protected set; }

        public bool UserNameLoaded { get; protected set; }

        public bool UserEmailLoaded { get; protected set; }

        public bool UserSecurityStampLoaded { get; protected set; }

        public bool UserClaimsLoaded { get; protected set; }

        public UserClaimsPrincipalLoader(UserManager<TUser, TContext> userManager, IOptions<IdentityOptions> optionsAccessor, IOptionsSnapshot<ClaimsLoadingOptions> claimsOptionsAccessor)
        {
            if (optionsAccessor.Value == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            UserManager = userManager;
            Options = optionsAccessor.Value;
            _claimsOptions = claimsOptionsAccessor.Value;
        }

        public virtual Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            if (User != null && UserClaims is ClaimsIdentity userClaims)
            {
                return Task.FromResult(new ClaimsPrincipal(userClaims));
            }
            User = user;
            UserClaims = new ClaimsIdentity("Identity.Application",
                Options.ClaimsIdentity.UserNameClaimType,
                Options.ClaimsIdentity.RoleClaimType);
            return CreateAsyncCore(user, UserClaims);
        }

        protected virtual async Task<ClaimsPrincipal> CreateAsyncCore(TUser user, ClaimsIdentity userClaims)
        {
            if (ClaimsOptions.LoadUserId)
            {
                await LoadUserIdAsyncCore(user, userClaims).ConfigureAwait(false);
            }
            if (ClaimsOptions.LoadUserName)
            {
                await LoadUserNameAsyncCore(user, userClaims).ConfigureAwait(false);
            }
            if (ClaimsOptions.LoadUserEmail && UserManager.SupportsUserEmail)
            {
                await LoadUserEmailAsyncCore(user, userClaims).ConfigureAwait(false);
            }
            if (ClaimsOptions.LoadUserSecurityStamp && UserManager.SupportsUserSecurityStamp)
            {
                await LoadUserSecurityStampAsyncCore(user, userClaims).ConfigureAwait(false);
            }
            if (ClaimsOptions.LoadUserClaims && UserManager.SupportsUserClaim)
            {
                await LoadUserClaimsAsyncCore(user, userClaims).ConfigureAwait(false);
            }
            return new ClaimsPrincipal(userClaims);
        }

        public Task LoadUserIdAsync()
        {
            if (User is not TUser user ||
                UserClaims is not ClaimsIdentity userClaims)
            {
                throw new InvalidOperationException();
            }
            if (UserIdLoaded)
            {
                return Task.CompletedTask;
            }
            return LoadUserIdAsyncCore(user, userClaims);
        }

        protected async Task LoadUserIdAsyncCore(TUser user, ClaimsIdentity userClaims)
        {
            string userId = await UserManager.GetUserIdAsync(user).ConfigureAwait(false);
            userClaims.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId));
            UserIdLoaded = true;
        }

        public Task LoadUserNameAsync()
        {
            if (User is not TUser user ||
                UserClaims is not ClaimsIdentity userClaims)
            {
                throw new InvalidOperationException();
            }
            if (UserNameLoaded)
            {
                return Task.CompletedTask;
            }
            return LoadUserNameAsyncCore(user, userClaims);
        }

        protected async Task LoadUserNameAsyncCore(TUser user, ClaimsIdentity userClaims)
        {
            string? userName = await UserManager.GetUserNameAsync(user).ConfigureAwait(false);
            userClaims.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, userName!));
            UserNameLoaded = true;
        }

        public Task LoadUserEmailAsync()
        {
            if (User is not TUser user ||
                UserClaims is not ClaimsIdentity userClaims)
            {
                throw new InvalidOperationException();
            }
            if (UserEmailLoaded)
            {
                return Task.CompletedTask;
            }
            return LoadUserEmailAsyncCore(user, userClaims);
        }

        protected async Task LoadUserEmailAsyncCore(TUser user, ClaimsIdentity userClaims)
        {
            string? userEmail = await UserManager.GetEmailAsync(user).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(userEmail))
            {
                userClaims.AddClaim(new Claim(Options.ClaimsIdentity.EmailClaimType, userEmail));
            }
            UserEmailLoaded = true;
        }

        public Task LoadUserSecurityStampAsync()
        {
            if (User is not TUser user ||
                UserClaims is not ClaimsIdentity userClaims)
            {
                throw new InvalidOperationException();
            }
            if (UserSecurityStampLoaded)
            {
                return Task.CompletedTask;
            }
            return LoadUserSecurityStampAsyncCore(user, userClaims);
        }

        protected async Task LoadUserSecurityStampAsyncCore(TUser user, ClaimsIdentity userClaims)
        {
            string userSecurityStamp = await UserManager.GetSecurityStampAsync(user).ConfigureAwait(false);
            userClaims.AddClaim(new Claim(Options.ClaimsIdentity.SecurityStampClaimType, userSecurityStamp));
            UserSecurityStampLoaded = true;
        }

        public Task LoadUserClaimsAsync()
        {
            if (User is not TUser user ||
                 UserClaims is not ClaimsIdentity userClaims)
            {
                throw new InvalidOperationException();
            }
            if (UserClaimsLoaded)
            {
                return Task.CompletedTask;
            }
            return LoadUserClaimsAsyncCore(user, userClaims);
        }

        protected async Task LoadUserClaimsAsyncCore(TUser user, ClaimsIdentity userClaims)
        {
            IList<Claim> claims = await UserManager.GetClaimsAsync(user).ConfigureAwait(false);
            userClaims.AddClaims(claims);
            UserClaimsLoaded = true;
        }
    }

    public sealed class DefaultUserClaimsPrincipalLoader<TUser, TContext> : UserClaimsPrincipalLoader<TUser, TContext> where TUser : class where TContext : DbContext
    {
        public DefaultUserClaimsPrincipalLoader(UserManager<TUser, TContext> userManager, IOptions<IdentityOptions> optionsAccessor, IOptionsSnapshot<ClaimsLoadingOptions> claimsOptionsAccessor) : base(userManager, optionsAccessor, claimsOptionsAccessor)
        {

        }
    }

    public interface IUserClaimsPrincipalLoader<TUser, TContext> : IUserClaimsPrincipalFactory<TUser, TContext> where TUser : class where TContext : DbContext
    {
        TUser? User { get; }

        UserManager<TUser> UserManager { get; }

        IdentityOptions Options { get; }

        bool UserIdLoaded { get; }

        bool UserNameLoaded { get; }

        bool UserEmailLoaded { get; }

        bool UserSecurityStampLoaded { get; }

        bool UserClaimsLoaded { get; }

        Task LoadUserIdAsync();

        Task LoadUserNameAsync();

        Task LoadUserEmailAsync();

        Task LoadUserSecurityStampAsync();

        Task LoadUserClaimsAsync();
    }

    public class UserClaimsPrincipalLoader<TUser, TRole, TContext> : UserClaimsPrincipalLoader<TUser, TContext>,
                                                                     IUserClaimsPrincipalLoader<TUser, TRole, TContext> where TUser : class where TRole : class where TContext : DbContext
    {
        public RoleManager<TRole> RoleManager { get; private set; }

        public bool UserRolesLoaded { get; protected set; }

        public bool RoleClaimsLoaded { get; protected set; }

        protected override RoleClaimsLoadingOptions ClaimsOptions => Unsafe.As<ClaimsLoadingOptions, RoleClaimsLoadingOptions>(ref Unsafe.AsRef(in _claimsOptions));

        public UserClaimsPrincipalLoader(UserManager<TUser, TContext> userManager, RoleManager<TRole, TContext> roleManager, IOptions<IdentityOptions> optionsAccessor, IOptionsSnapshot<RoleClaimsLoadingOptions> claimsOptionsAccessor) : base(userManager, optionsAccessor, claimsOptionsAccessor)
        {
            RoleManager = roleManager;
        }

        protected override async Task<ClaimsPrincipal> CreateAsyncCore(TUser user, ClaimsIdentity userClaims)
        {
            ClaimsPrincipal id = await base.CreateAsyncCore(user, userClaims).ConfigureAwait(false);
            if (ClaimsOptions.LoadUserRoles && UserManager.SupportsUserRole)
            {
                await LoadUserRolesAsyncCore(user, userClaims).ConfigureAwait(false);
                if (ClaimsOptions.LoadRoleClaims && RoleManager.SupportsRoleClaims)
                {
                    await LoadRoleClaimsAsyncCore(user, userClaims).ConfigureAwait(false);
                }
            }
            return id;
        }

        public Task LoadUserRolesAsync()
        {
            if (User is not TUser user ||
                UserClaims is not ClaimsIdentity userClaims)
            {
                throw new InvalidOperationException();
            }
            if (UserRolesLoaded)
            {
                return Task.CompletedTask;
            }
            return LoadUserRolesAsyncCore(user, userClaims);
        }

        public async Task LoadUserRolesAsyncCore(TUser user, ClaimsIdentity id)
        {
            var roles = await UserManager.GetRolesAsync(user).ConfigureAwait(false);
            foreach (var roleName in roles)
            {
                id.AddClaim(new Claim(Options.ClaimsIdentity.RoleClaimType, roleName));
            }
            UserRolesLoaded = true;
        }

        public Task LoadRoleClaimsAsync()
        {
            if (User is not TUser user ||
                UserClaims is not ClaimsIdentity userClaims)
            {
                throw new InvalidOperationException();
            }
            if (RoleClaimsLoaded)
            {
                return Task.CompletedTask;
            }
            return LoadRoleClaimsAsyncCore(user, userClaims);
        }

        public async Task LoadRoleClaimsAsyncCore(TUser user, ClaimsIdentity id)
        {
            if (!UserRolesLoaded)
            {
                await LoadUserRolesAsyncCore(user, id).ConfigureAwait(false);
            }
            foreach (Claim claim in id.Claims)
            {
                if (claim.Type == Options.ClaimsIdentity.RoleClaimType)
                {
                    TRole? role = await RoleManager.FindByNameAsync(claim.Value).ConfigureAwait(false);
                    if (role != null)
                    {
                        id.AddClaims(await RoleManager.GetClaimsAsync(role).ConfigureAwait(false));
                    }
                }
            }
            RoleClaimsLoaded = true;
        }
    }

    public sealed class DefaultUserClaimsPrincipalLoader<TUser, TRole, TContext> : UserClaimsPrincipalLoader<TUser, TRole, TContext> where TUser : class where TRole : class where TContext : DbContext
    {
        public DefaultUserClaimsPrincipalLoader(UserManager<TUser, TContext> userManager, RoleManager<TRole, TContext> roleManager, IOptions<IdentityOptions> optionsAccessor, IOptionsSnapshot<RoleClaimsLoadingOptions> claimsOptionsAccessor) : base(userManager, roleManager, optionsAccessor, claimsOptionsAccessor)
        {

        }
    }
}
