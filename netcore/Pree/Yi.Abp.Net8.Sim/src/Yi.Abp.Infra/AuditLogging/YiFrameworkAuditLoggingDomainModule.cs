using Volo.Abp.Auditing;
using Volo.Abp.Domain;

namespace Yi.Abp.Infra.AuditLogging
{
    [DependsOn(typeof(YiFrameworkAuditLoggingDomainSharedModule),
        
        
        typeof(AbpDddDomainModule),
        typeof(AbpAuditingModule)
        )]
    public class YiFrameworkAuditLoggingDomainModule:AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

        }
    }
}
