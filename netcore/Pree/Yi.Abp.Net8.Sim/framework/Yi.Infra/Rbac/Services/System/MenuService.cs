using SqlSugar;
using Volo.Abp.Application.Dtos;
using Yi.Framework.Ddd.Application;
using Yi.Framework.SqlSugarCore.Abstractions;
using Yi.Infra.Rbac.Dtos.Menu;
using Yi.Infra.Rbac.Entities;
using Yi.Infra.Rbac.IServices;

namespace Yi.Infra.Rbac.Services.System;

/// <summary>
///     Menu服务实现
/// </summary>
public class MenuService : YiCrudAppService<MenuAggregateRoot, MenuGetOutputDto, MenuGetListOutputDto, Guid,
        MenuGetListInputVo, MenuCreateInputVo, MenuUpdateInputVo>,
    IMenuService
{
    private readonly ISqlSugarRepository<MenuAggregateRoot, Guid> _repository;

    public MenuService(ISqlSugarRepository<MenuAggregateRoot, Guid> repository) : base(repository)
    {
        _repository = repository;
    }

    public override async Task<PagedResultDto<MenuGetListOutputDto>> GetListAsync(MenuGetListInputVo input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository._DbQueryable.WhereIF(!string.IsNullOrEmpty(input.MenuName),
                x => x.MenuName.Contains(input.MenuName!))
            .WhereIF(input.State is not null, x => x.State == input.State)
            .OrderByDescending(x => x.OrderNum)
            .ToListAsync();
        //.ToPageListAsync(input.SkipCount, input.MaxResultCount, total);
        return new PagedResultDto<MenuGetListOutputDto>(total, await MapToGetListOutputDtosAsync(entities));
    }

    /// <summary>
    ///     查询当前角色的菜单
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<List<MenuGetListOutputDto>> GetListRoleIdAsync(Guid roleId)
    {
        var entities = await _repository._DbQueryable.Where(m =>
                SqlFunc.Subqueryable<RoleMenuEntity>().Where(rm => rm.RoleId == roleId && rm.MenuId == m.Id).Any())
            .ToListAsync();

        return await MapToGetListOutputDtosAsync(entities);
    }
}