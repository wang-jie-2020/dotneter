using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Yi.System.Domains.System;

public static class TenantManagementExtensions
{
    public static IDisposable ChangeDefault(this ICurrentTenant currentTenant)
    {
        return currentTenant.Change(null, ConnectionStrings.DefaultConnectionStringName);
    }
}