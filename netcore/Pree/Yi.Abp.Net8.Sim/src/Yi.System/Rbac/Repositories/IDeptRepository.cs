using Yi.System.Rbac.Entities;

namespace Yi.System.Rbac.Repositories;

public interface IDeptRepository : ISqlSugarRepository<DeptAggregateRoot, Guid>
{
    Task<List<Guid>> GetChildListAsync(Guid deptId);
    
    Task<List<DeptAggregateRoot>> GetListRoleIdAsync(Guid roleId);
}