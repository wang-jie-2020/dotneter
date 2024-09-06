using Yi.Abp.Infra.Rbac.Entities;
using Yi.Framework.SqlSugarCore.Abstractions;

namespace Yi.Abp.Infra.Rbac.Repositories
{
    public interface IDeptRepository : ISqlSugarRepository<DeptAggregateRoot, Guid>
    {
        Task<List<Guid>> GetChildListAsync(Guid deptId);
        Task<List<DeptAggregateRoot>> GetListRoleIdAsync(Guid roleId);
    }
}
