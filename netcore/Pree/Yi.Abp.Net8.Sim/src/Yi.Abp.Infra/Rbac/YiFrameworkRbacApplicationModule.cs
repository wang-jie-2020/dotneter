using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.BackgroundWorkers.Quartz;
using Yi.Framework.Ddd.Application;

namespace Yi.Abp.Infra.Rbac
{
    [DependsOn(
        typeof(YiFrameworkRbacApplicationContractsModule),
        typeof(YiFrameworkRbacDomainModule),


        typeof(YiFrameworkDddApplicationModule),
       typeof(AbpBackgroundWorkersQuartzModule)
        )]
    public class YiFrameworkRbacApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var service = context.Services;

            service.AddCaptcha();
        }

        public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
        {
        }
    }
}
