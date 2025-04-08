using Yi.Sys.Domains.Infra.Entities;

namespace Yi.Sys.Domains.Infra.Repositories;

public interface IDeptRepository : ISqlSugarRepository<DeptEntity, Guid>
{
    Task<List<Guid>> GetChildListAsync(Guid deptId);
    
    Task<List<DeptEntity>> GetListRoleIdAsync(Guid roleId);
}