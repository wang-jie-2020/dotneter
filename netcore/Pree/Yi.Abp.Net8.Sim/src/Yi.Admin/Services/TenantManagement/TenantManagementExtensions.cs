using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Yi.Admin.Services.TenantManagement;

public static class TenantManagementExtensions
{
    public static IDisposable ChangeDefault(this ICurrentTenant currentTenant)
    {
        return currentTenant.Change(null, ConnectionStrings.DefaultConnectionStringName);
    }
}