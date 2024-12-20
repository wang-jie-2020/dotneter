using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.Sys.Domains.Infra.Entities;
using Yi.Sys.Services.Infra.Dtos;

namespace Yi.Sys.Services.Infra.Impl;

public class MenuService : ApplicationService, IMenuService
{
    private readonly ISqlSugarRepository<MenuEntity, Guid> _repository;

    public MenuService(ISqlSugarRepository<MenuEntity, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<MenuDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<MenuDto>();
    }

    public async Task<PagedResultDto<MenuDto>> GetListAsync(MenuGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.DbQueryable.WhereIF(!string.IsNullOrEmpty(input.MenuName),
                x => x.MenuName.Contains(input.MenuName!))
            .WhereIF(input.State is not null, x => x.State == input.State)
            .OrderByDescending(x => x.OrderNum)
            .ToListAsync();

        return new PagedResultDto<MenuDto>(total, entities.Adapt<List<MenuDto>>());
    }

    public async Task<MenuDto> CreateAsync(MenuCreateInput input)
    {
        var entity = input.Adapt<MenuEntity>();
        await _repository.InsertAsync(entity, autoSave: true);

        return entity.Adapt<MenuDto>();
    }

    public async Task<MenuDto> UpdateAsync(Guid id, MenuUpdateInput input)
    {
        var entity = await _repository.GetAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity, autoSave: true);

        return entity.Adapt<MenuDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }
    
    /// <summary>
    ///     查询当前角色的菜单
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<List<MenuDto>> GetListRoleIdAsync(Guid roleId)
    {
        var entities = await _repository.DbQueryable.Where(m =>
                SqlFunc.Subqueryable<RoleMenuEntity>().Where(rm => rm.RoleId == roleId && rm.MenuId == m.Id).Any())
            .ToListAsync();

        return entities.Adapt<List<MenuDto>>();
    }
}