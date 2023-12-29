using Hugin.Infrastructure.Exporting;
using Hugin.Infrastructure.Exporting.Magicodes;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Hugin.Platform
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule),
        typeof(PlatformDomainModule),
        typeof(PlatformApplicationContractsModule),
        typeof(PlatformBackgroundJobModule),
        typeof(PlatformCapModule)
        )]
    public class PlatformApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<PlatformApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<PlatformApplicationModule>();
            });

            context.Services.AddTransient<IExcelExporting, ExcelExporting>();
        }
    }
}
