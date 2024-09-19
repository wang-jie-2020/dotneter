using Yi.System.Domains.Rbac.Entities;

namespace Yi.System.Domains.Rbac.Repositories;

public interface IDeptRepository : ISqlSugarRepository<DeptEntity, Guid>
{
    Task<List<Guid>> GetChildListAsync(Guid deptId);
    
    Task<List<DeptEntity>> GetListRoleIdAsync(Guid roleId);
}