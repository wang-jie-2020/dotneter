﻿using Yi.Sys.Domains.Infra.Entities;

namespace Yi.Sys.Domains.Infra.Repositories;

public interface ISqlSugarTenantRepository : ISqlSugarRepository<TenantEntity, Guid>
{
    Task<TenantEntity> FindByNameAsync(string name, bool includeDetails = true);

    Task<List<TenantEntity>> GetListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false);
    
    Task<long> GetCountAsync(
        string? filter = null);
}