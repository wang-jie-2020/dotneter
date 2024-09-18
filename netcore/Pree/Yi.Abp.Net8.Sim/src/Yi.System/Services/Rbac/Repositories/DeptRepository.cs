using Volo.Abp.DependencyInjection;
using Yi.System.Services.Rbac.Entities;

namespace Yi.System.Services.Rbac.Repositories;

public class DeptRepository : SqlSugarRepository<DeptAggregateRoot, Guid>, IDeptRepository, ITransientDependency
{
    public DeptRepository(ISugarDbContextProvider<ISqlSugarDbContext> sugarDbContextProvider) : base(
        sugarDbContextProvider)
    {
    }

    public async Task<List<Guid>> GetChildListAsync(Guid deptId)
    {
        var entities = await DbQueryable.ToChildListAsync(x => x.ParentId, deptId);
        return entities.Select(x => x.Id).ToList();
    }

    public async Task<List<DeptAggregateRoot>> GetListRoleIdAsync(Guid roleId)
    {
        return await DbQueryable.Where(d =>
                SqlFunc.Subqueryable<RoleDeptEntity>().Where(rd => rd.RoleId == roleId && d.Id == rd.DeptId).Any())
            .ToListAsync();
    }
}