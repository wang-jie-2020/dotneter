using Volo.Abp.DependencyInjection;
using Yi.Framework.SqlSugarCore;
using Yi.Framework.SqlSugarCore.Repositories;
using Yi.System.Domains.Entities;

namespace Yi.System.Domains.Repositories;

public class DeptRepository : SqlSugarRepository<DeptEntity>, IDeptRepository, ITransientDependency
{
    public DeptRepository(ISugarDbContextProvider<ISqlSugarDbContext> sugarDbContextProvider) : base(sugarDbContextProvider)
    {
    }

    public async Task<List<Guid>> GetChildListAsync(Guid deptId)
    {
        var entities = await AsQueryable().ToChildListAsync(x => x.ParentId, deptId);
        return entities.Select(x => x.Id).ToList();
    }

    public async Task<List<DeptEntity>> GetListRoleIdAsync(Guid roleId)
    {
        return await AsQueryable().
            Where(d => SqlFunc.Subqueryable<RoleDeptEntity>().Where(rd => rd.RoleId == roleId && d.Id == rd.DeptId).Any())
            .ToListAsync();
    }
}