using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.Framework.Core.Helper;
using Yi.Infra.Rbac.Dtos;
using Yi.Infra.Rbac.Entities;
using Yi.Infra.Rbac.Repositories;

namespace Yi.Infra.Rbac.Services;

[RemoteService(false)]
public class DeptService : ApplicationService, IDeptService
{
    private readonly IDeptRepository _repository;

    public DeptService(IDeptRepository repository)
    {
        _repository = repository;
    }

    public async Task<DeptGetOutputDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<DeptGetOutputDto>();
    }

    public async Task<PagedResultDto<DeptGetListOutputDto>> GetListAsync(DeptGetListInput input)
    {
        RefAsync<int> total = 0;
        var entities = await _repository._DbQueryable
            .WhereIF(!string.IsNullOrEmpty(input.DeptName), u => u.DeptName.Contains(input.DeptName!))
            .WhereIF(input.State is not null, u => u.State == input.State)
            .OrderBy(u => u.OrderNum)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);

        return new PagedResultDto<DeptGetListOutputDto>
        {
            TotalCount = total,
            Items = entities.Adapt<List<DeptGetListOutputDto>>()
        };
    }

    public async Task<DeptGetOutputDto> CreateAsync(DeptCreateInput input)
    {
        var entity = input.Adapt<DeptAggregateRoot>();
        await _repository.InsertAsync(entity, autoSave: true);

        return entity.Adapt<DeptGetOutputDto>();
    }

    public async Task<DeptGetOutputDto> UpdateAsync(Guid id, DeptUpdateInput input)
    {
        var entity = await _repository.GetAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity, autoSave: true);

        return entity.Adapt<DeptGetOutputDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }

    public async Task<IActionResult> GetExportExcelAsync(DeptGetListInput input)
    {
        if (input is IPagedResultRequest paged)
        {
            paged.SkipCount = 0;
            paged.MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount;
        }

        var output = await GetListAsync(input);
        return new PhysicalFileResult(ExporterHelper.ExportExcel(output.Items), "application/vnd.ms-excel");
    }

    public async Task<List<Guid>> GetChildListAsync(Guid deptId)
    {
        return await _repository.GetChildListAsync(deptId);
    }
    
    public async Task<List<DeptGetListOutputDto>> GetRoleIdAsync(Guid roleId)
    {
        var entities = await _repository.GetListRoleIdAsync(roleId);
        return entities.Adapt<List<DeptGetListOutputDto>>();
    }
}