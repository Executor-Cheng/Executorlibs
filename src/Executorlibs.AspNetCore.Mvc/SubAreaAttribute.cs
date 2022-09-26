namespace Executorlibs.AspNetCore.Mvc
{
    public sealed class SubAreaAttribute : SubComponentAttribute
    {
        public string SubAreaName { get; }

        public override string SubComponentName => SubAreaName;

        public SubAreaAttribute(string subAreaName)
        {
            SubAreaName = subAreaName;
        }
    }
}
