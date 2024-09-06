﻿using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Yi.Abp.Infra.TenantManagement
{
    public static class TenantManagementExtensions
    {
        public static IDisposable ChangeDefalut(this ICurrentTenant currentTenant)
        {
            return currentTenant.Change(null, ConnectionStrings.DefaultConnectionStringName);
        }
    }
}
