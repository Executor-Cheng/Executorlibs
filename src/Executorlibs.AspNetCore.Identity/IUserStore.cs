using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Executorlibs.AspNetCore.Identity
{
    public interface IUserStore<TUser, TContext> : IUserStore<TUser> where TUser : class where TContext : DbContext
    {

    }
}
