namespace Executorlibs.AspNetCore.Mvc.ApplicationModels
{
    public class SubControllerApplicationModelProvider : SubComponentApplicationModelProvider<SubControllerAttribute>
    {
        public override int Order => -1000;

        protected override string SubComponentName => "subcontroller";
    }
}
