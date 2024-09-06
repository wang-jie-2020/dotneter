using Volo.Abp.Domain;

namespace Yi.Abp.Infra.AuditLogging
{
    [DependsOn(typeof(AbpDddDomainSharedModule))]
    public class YiFrameworkAuditLoggingDomainSharedModule:AbpModule
    {

    }
}
