using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;
using Volo.Abp.MultiStadium.ConfigurationStore;
using Volo.Abp.Security;

namespace Volo.Abp.MultiStadium
{
    [DependsOn(
        typeof(AbpDataModule),
        typeof(AbpSecurityModule)
    )]
    public class AbpMultiStadiumModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpDefaultStadiumStoreOptions>(configuration);
        }
    }
}