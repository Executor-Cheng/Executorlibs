using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Executorlibs.AspNetCore.Identity
{
    public interface IRoleStore<TRole, TContext> : IRoleStore<TRole> where TRole : class where TContext : DbContext
    {

    }
}
