using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Hugin.IdentityServer
{
    [Dependency(ReplaceServices = true)]
    public class ProjectBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Hugin";
    }
}
