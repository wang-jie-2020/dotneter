using Yi.System.Domains.Entities;

namespace Yi.System.Domains.Repositories;

public interface IDeptRepository : ISqlSugarRepository<DeptEntity, Guid>
{
    Task<List<Guid>> GetChildListAsync(Guid deptId);
    
    Task<List<DeptEntity>> GetListRoleIdAsync(Guid roleId);
}