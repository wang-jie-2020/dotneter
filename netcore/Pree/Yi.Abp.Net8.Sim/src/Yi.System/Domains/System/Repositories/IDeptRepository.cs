using Yi.System.Domains.System.Entities;

namespace Yi.System.Domains.System.Repositories;

public interface IDeptRepository : ISqlSugarRepository<DeptEntity, Guid>
{
    Task<List<Guid>> GetChildListAsync(Guid deptId);
    
    Task<List<DeptEntity>> GetListRoleIdAsync(Guid roleId);
}