using Yi.System.Services.Rbac.Entities;

namespace Yi.System.Services.Rbac.Repositories;

public interface IDeptRepository : ISqlSugarRepository<DeptAggregateRoot, Guid>
{
    Task<List<Guid>> GetChildListAsync(Guid deptId);
    
    Task<List<DeptAggregateRoot>> GetListRoleIdAsync(Guid roleId);
}