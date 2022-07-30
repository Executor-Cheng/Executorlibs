namespace Executorlibs.AspNetCore.Mvc
{
    public sealed class SubControllerAttribute : SubComponentAttribute
    {
        public string SubControllerName { get; }

        public override string SubComponentName => SubControllerName;

        public SubControllerAttribute(string subControllerName)
        {
            SubControllerName = subControllerName;
        }
    }
}
