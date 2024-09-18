using Volo.Abp.Auditing;
using Volo.Abp.BackgroundWorkers.Quartz;

namespace Yi.Admin;

[DependsOn(
    typeof(AbpAuditingModule),
    typeof(AbpBackgroundWorkersQuartzModule)
)]
public class YiAdminModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        
    }
}