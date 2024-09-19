﻿using Yi.System.Domains.Rbac.Entities;

namespace Yi.System.Domains.Rbac.Repositories;

public interface IDeptRepository : ISqlSugarRepository<DeptAggregateRoot, Guid>
{
    Task<List<Guid>> GetChildListAsync(Guid deptId);
    
    Task<List<DeptAggregateRoot>> GetListRoleIdAsync(Guid roleId);
}