using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Hugin.Terminal
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(AbpAutoMapperModule),
        typeof(TerminalDomainSharedModule)
    )]
    public class TerminalDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<TerminalDomainModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<TerminalDomainModule>(validate: false);
            });
        }
    }
}
