using Yi.AspNetCore;
using Yi.AspNetCore.MultiTenancy;

namespace Yi.System.Domains;

public static class TenantManagementExtensions
{
    public static IDisposable ChangeDefault(this ICurrentTenant currentTenant)
    {
        return currentTenant.Change(null, ConnectionStrings.DefaultConnectionStringName);
    }
}