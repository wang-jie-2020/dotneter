using Yi.Framework.SqlSugarCore;
using Yi.Infra.Rbac.Entities;

namespace Yi.Infra.Rbac.Repositories;

public interface IDeptRepository : ISqlSugarRepository<DeptAggregateRoot, Guid>
{
    Task<List<Guid>> GetChildListAsync(Guid deptId);
    Task<List<DeptAggregateRoot>> GetListRoleIdAsync(Guid roleId);
}