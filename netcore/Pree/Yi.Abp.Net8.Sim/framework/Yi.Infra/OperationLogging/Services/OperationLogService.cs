using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.Framework.Core.Helper;
using Yi.Framework.SqlSugarCore;
using Yi.Infra.OperationLogging.Dtos;
using Yi.Infra.OperationLogging.Entities;

namespace Yi.Infra.OperationLogging.Services;

[RemoteService(false)]
public class OperationLogService : ApplicationService, IOperationLogService
{
    private readonly ISqlSugarRepository<OperationLogEntity, Guid> _repository;

    public OperationLogService(ISqlSugarRepository<OperationLogEntity, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<OperationLogDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<OperationLogDto>();
    }

    public async Task<PagedResultDto<OperationLogDto>> GetListAsync(OperationLogGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository._DbQueryable.WhereIF(!string.IsNullOrEmpty(input.OperUser),
                x => x.OperUser.Contains(input.OperUser!))
            .WhereIF(input.OperType is not null, x => x.OperType == input.OperType)
            .WhereIF(input.StartTime is not null && input.EndTime is not null,
                x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);

        return new PagedResultDto<OperationLogDto>(total, entities.Adapt<List<OperationLogDto>>());
    }

    public async Task DeleteAsync([FromBody] IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }

    public async Task<IActionResult> GetExportExcelAsync(OperationLogGetListInput input)
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