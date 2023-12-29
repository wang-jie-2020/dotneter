using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Hugin.Identity
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(IdentityApplicationContractsModule)
    )]
    public class IdentityHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(IdentityApplicationContractsModule).Assembly,
                "Default"
            );
        }
    }
}
