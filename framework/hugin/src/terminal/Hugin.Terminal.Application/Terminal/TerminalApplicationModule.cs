using Hugin.Platform;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Hugin.Terminal
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule),
        typeof(TerminalDomainModule),
        typeof(TerminalApplicationContractsModule),
        typeof(TerminalBackgroundJobModule),
        typeof(TerminalCapModule)
        )]
    public class TerminalApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<TerminalApplicationModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<TerminalApplicationModule>();
            });
        }
    }
}
