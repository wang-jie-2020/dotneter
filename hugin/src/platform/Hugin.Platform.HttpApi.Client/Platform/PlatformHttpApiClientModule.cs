using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Hugin.Platform
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(PlatformApplicationContractsModule))]
    public class PlatformHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = PlatformConsts.Name;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(PlatformApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
