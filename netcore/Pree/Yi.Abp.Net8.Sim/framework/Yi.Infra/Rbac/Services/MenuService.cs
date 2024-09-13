using SqlSugar;
using Volo.Abp.Application.Dtos;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.Rbac.Dtos;
using Yi.Infra.Rbac.Entities;

namespace Yi.Infra.Rbac.Services;

/// <summary>
///     Menu服务实现
/// </summary>
public class MenuService : YiCrudAppService<MenuAggregateRoot, MenuDto, MenuDto, Guid,
        MenuGetListInput, MenuCreateInput, MenuUpdateInput>,
    IMenuService
{
    private readonly ISqlSugarRepository<MenuAggregateRoot, Guid> _repository;

    public MenuService(ISqlSugarRepository<MenuAggregateRoot, Guid> repository) : base(repository)
    {
        _repository = repository;
    }

    public override async Task<PagedResultDto<MenuDto>> GetListAsync(MenuGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository._DbQueryable.WhereIF(!string.IsNullOrEmpty(input.MenuName),
                x => x.MenuName.Contains(input.MenuName!))
            .WhereIF(input.State is not null, x => x.State == input.State)
            .OrderByDescending(x => x.OrderNum)
            .ToListAsync();
        //.ToPageListAsync(input.SkipCount, input.MaxResultCount, total);
        return new PagedResultDto<MenuDto>(total, await MapToGetListOutputDtosAsync(entities));
    }

    /// <summary>
    ///     查询当前角色的菜单
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<List<MenuDto>> GetListRoleIdAsync(Guid roleId)
    {
        var entities = await _repository._DbQueryable.Where(m =>
                SqlFunc.Subqueryable<RoleMenuEntity>().Where(rm => rm.RoleId == roleId && rm.MenuId == m.Id).Any())
            .ToListAsync();

        return await MapToGetListOutputDtosAsync(entities);
    }
}