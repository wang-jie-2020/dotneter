using Yi.Framework.Abstractions;
using Yi.Framework.Core.Entities;
using Yi.System.Services.Dtos;

namespace Yi.System.Services.Impl;

public class MenuService : BaseService, IMenuService
{
    private readonly ISqlSugarRepository<MenuEntity> _repository;

    public MenuService(ISqlSugarRepository<MenuEntity> repository)
    {
        _repository = repository;
    }

    public async Task<MenuDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<MenuDto>();
    }

    public async Task<PagedResult<MenuDto>> GetListAsync(MenuQuery query)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.AsQueryable().WhereIF(!string.IsNullOrEmpty(query.MenuName),
                x => x.MenuName.Contains(query.MenuName!))
            .WhereIF(query.State is not null, x => x.State == query.State)
            .OrderByDescending(x => x.OrderNum)
            .ToListAsync();

        return new PagedResult<MenuDto>(total, entities.Adapt<List<MenuDto>>());
    }

    public async Task<MenuDto> CreateAsync(MenuInput input)
    {
        var entity = input.Adapt<MenuEntity>();
        await _repository.InsertAsync(entity);

        return entity.Adapt<MenuDto>();
    }

    public async Task<MenuDto> UpdateAsync(Guid id, MenuInput input)
    {
        var entity = await _repository.GetByIdAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity);

        return entity.Adapt<MenuDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }
    
    /// <summary>
    ///     查询当前角色的菜单
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<List<MenuDto>> GetListRoleIdAsync(Guid roleId)
    {
        var entities = await _repository.AsQueryable().Where(m =>
                SqlFunc.Subqueryable<RoleMenuEntity>().Where(rm => rm.RoleId == roleId && rm.MenuId == m.Id).Any())
            .ToListAsync();

        return entities.Adapt<List<MenuDto>>();
    }
}