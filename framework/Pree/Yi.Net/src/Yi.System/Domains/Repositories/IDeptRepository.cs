using Yi.System.Domains.Entities;

namespace Yi.System.Domains.Repositories;

public interface IDeptRepository : ISqlSugarRepository<DeptEntity>
{
    Task<List<Guid>> GetChildListAsync(Guid deptId);
    
    Task<List<DeptEntity>> GetListRoleIdAsync(Guid roleId);
}