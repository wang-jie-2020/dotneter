using Yi.Abp.Infra.AuditLogging;
using Yi.Abp.Infra.Rbac;
using Yi.Abp.Infra.TenantManagement;

namespace Yi.Abp.Infra;

[DependsOn(
    typeof(YiFrameworkAuditLoggingModule),
    typeof(YiFrameworkRbacModule),
    typeof(YiFrameworkTenantManagementModule)
)]
public class YiFrameworkInfraModule : AbpModule
{
}