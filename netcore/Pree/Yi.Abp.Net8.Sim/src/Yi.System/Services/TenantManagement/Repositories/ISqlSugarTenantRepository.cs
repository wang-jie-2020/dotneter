﻿using Yi.System.Services.TenantManagement.Entities;

namespace Yi.System.Services.TenantManagement.Repositories;

public interface ISqlSugarTenantRepository : ISqlSugarRepository<TenantAggregateRoot, Guid>
{
    Task<TenantAggregateRoot> FindByNameAsync(string name, bool includeDetails = true);

    Task<List<TenantAggregateRoot>> GetListAsync(string sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string filter = null,
        bool includeDetails = false);
    
    Task<long> GetCountAsync(
        string filter = null);
}