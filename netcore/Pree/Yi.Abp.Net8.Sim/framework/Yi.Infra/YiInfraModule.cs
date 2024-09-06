using Yi.Infra.AuditLogging;
using Yi.Infra.Rbac;
using Yi.Infra.TenantManagement;

namespace Yi.Infra;

[DependsOn(
    typeof(YiFrameworkAuditLoggingModule),
    typeof(YiFrameworkRbacModule),
    typeof(YiFrameworkTenantManagementModule)
)]
public class YiInfraModule : AbpModule
{
}