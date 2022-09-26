using System;

namespace Executorlibs.AspNetCore.Identity
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IdentityDbContextAttribute : Attribute
    {
        public Type DbContextType { get; }

        public IdentityDbContextAttribute(Type dbContextType)
        {
            DbContextType = dbContextType;
        }
    }
}
