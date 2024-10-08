using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.Framework.Core.Helper;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.Settings.Dtos;
using Yi.Infra.Settings.Entities;

namespace Yi.Infra.Settings.Services;

[RemoteService(false)]
public class ConfigService : ApplicationService, IConfigService
{
    private readonly ISqlSugarRepository<ConfigAggregateRoot, Guid> _repository;

    public ConfigService(ISqlSugarRepository<ConfigAggregateRoot, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<ConfigGetOutputDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<ConfigGetOutputDto>();
    }

    public async Task<PagedResultDto<ConfigGetListOutputDto>> GetListAsync(ConfigGetListInputVo input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository._DbQueryable.WhereIF(!string.IsNullOrEmpty(input.ConfigKey),
                x => x.ConfigKey.Contains(input.ConfigKey!))
            .WhereIF(!string.IsNullOrEmpty(input.ConfigName), x => x.ConfigName!.Contains(input.ConfigName!))
            .WhereIF(input.StartTime is not null && input.EndTime is not null,
                x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);

        return new PagedResultDto<ConfigGetListOutputDto>(total, entities.Adapt<List<ConfigGetListOutputDto>>());
    }

    public async Task<ConfigGetOutputDto> CreateAsync(ConfigCreateInputVo input)
    {
        var entity = input.Adapt<ConfigAggregateRoot>();
        await _repository.InsertAsync(entity, autoSave: true);

        return entity.Adapt<ConfigGetOutputDto>();
    }

    public async Task<ConfigGetOutputDto> UpdateAsync(Guid id, ConfigUpdateInput input)
    {
        var entity = await _repository.GetAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity, autoSave: true);

        return entity.Adapt<ConfigGetOutputDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }

    public async Task<IActionResult> GetExportExcelAsync(ConfigGetListInputVo input)
    {
        if (input is IPagedResultRequest paged)
        {
            paged.SkipCount = 0;
            paged.MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount;
        }

        var output = await GetListAsync(input);
        return new PhysicalFileResult(ExporterHelper.ExportExcel(output.Items), "application/vnd.ms-excel");
    }
}