using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Executorlibs.AspNetCore.Identity
{
    public class SignInManager<TUser, TContext> : SignInManager<TUser> where TUser : class where TContext : DbContext
    {
        public SignInManager(UserManager<TUser, TContext> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<TUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {

        }
    }

    public sealed class DefaultSignInManager<TUser, TContext> : SignInManager<TUser, TContext> where TUser : class where TContext : DbContext
    {
        public DefaultSignInManager(UserManager<TUser, TContext> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactoryProvider<TUser, TContext> claimsFactoryProvider, IOptions<IdentityOptions> optionsAccessor, ILogger<DefaultSignInManager<TUser, TContext>> logger, IAuthenticationSchemeProvider schemes, IUserConfirmation<TUser> confirmation) : base(userManager, contextAccessor, claimsFactoryProvider.GetFactory(), optionsAccessor, logger, schemes, confirmation)
        {

        }
    }
}
