﻿using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Yi.Infra.TenantManagement;

public static class TenantManagementExtensions
{
    public static IDisposable ChangeDefault(this ICurrentTenant currentTenant)
    {
        return currentTenant.Change(null, ConnectionStrings.DefaultConnectionStringName);
    }
}