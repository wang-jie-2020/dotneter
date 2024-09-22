using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.AspNetCore.Helpers;
using Yi.System.Domains.System.Entities;
using Yi.System.Services.System.Dtos;

namespace Yi.System.Services.System.Impl;

public class NoticeService : ApplicationService, INoticeService
{
    private readonly ISqlSugarRepository<NoticeEntity, Guid> _repository;

    public NoticeService(ISqlSugarRepository<NoticeEntity, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<NoticeDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<NoticeDto>();
    }

    public async Task<PagedResultDto<NoticeDto>> GetListAsync(NoticeGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.DbQueryable.WhereIF(input.Type is not null, x => x.Type == input.Type)
            .WhereIF(!string.IsNullOrEmpty(input.Title), x => x.Title!.Contains(input.Title!))
            .WhereIF(input.StartTime is not null && input.EndTime is not null, x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);

        return new PagedResultDto<NoticeDto>(total, entities.Adapt<List<NoticeDto>>());
    }

    public async Task<NoticeDto> CreateAsync(NoticeCreateInput input)
    {
        var entity = input.Adapt<NoticeEntity>();
        await _repository.InsertAsync(entity, autoSave: true);

        return entity.Adapt<NoticeDto>();
    }

    public async Task<NoticeDto> UpdateAsync(Guid id, NoticeUpdateInput input)
    {
        var entity = await _repository.GetAsync(id);
        input.Adapt(entity);
        await _repository.UpdateAsync(entity, autoSave: true);

        return entity.Adapt<NoticeDto>();
    }

    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }

    public async Task<IActionResult> GetExportExcelAsync(NoticeGetListInput input)
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