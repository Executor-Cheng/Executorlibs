namespace Executorlibs.AspNetCore.Mvc.ApplicationModels
{
    public class SubAreaApplicationModelProvider : SubComponentApplicationModelProvider<SubAreaAttribute>
    {
        public override int Order => -1000;

        protected override string SubComponentName => "subarea";
    }
}
