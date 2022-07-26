using System;

namespace Executorlibs.AspNetCore.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class SubComponentAttribute : Attribute
    {
        public abstract string SubComponentName { get; }
    }
}
