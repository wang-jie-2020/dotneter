using Yi.System.Domains.Sys.Entities;

namespace Yi.System.Domains.Sys.Repositories;

public interface IDeptRepository : ISqlSugarRepository<DeptEntity, Guid>
{
    Task<List<Guid>> GetChildListAsync(Guid deptId);
    
    Task<List<DeptEntity>> GetListRoleIdAsync(Guid roleId);
}