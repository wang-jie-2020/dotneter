using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.AspNetCore.Helpers;
using Yi.System.Domains.Rbac.Entities;
using Yi.System.Services.Rbac.Dtos;

namespace Yi.System.Services.Rbac;

[RemoteService((false))]
public class MenuService : ApplicationService, IMenuService
{
    private readonly ISqlSugarRepository<MenuAggregateRoot, Guid> _repository;

    public MenuService(ISqlSugarRepository<MenuAggregateRoot, Guid> repository)
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
        var entity = input.Adapt<MenuAggregateRoot>();
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

    public async Task<IActionResult> GetExportExcelAsync(MenuGetListInput input)
    {
        if (input is IPagedResultRequest paged)
        {
            paged.SkipCount = 0;
            paged.MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount;
        }

        var output = await GetListAsync(input);
        return new PhysicalFileResult(ExporterHelper.ExportExcel(output.Items), "application/vnd.ms-excel");
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