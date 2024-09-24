using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Yi.AspNetCore.Helpers;
using Yi.AspNetCore.System;
using Yi.System.Domains.Monitor.Entities;
using Yi.System.Services.Monitor.Dtos;

namespace Yi.System.Services.Monitor.Impl;

public class OperLogService : ApplicationService, IOperLogService
{
    private readonly ISqlSugarRepository<OperLogEntity, Guid> _repository;

    public OperLogService(ISqlSugarRepository<OperLogEntity, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<OperLogDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<OperLogDto>();
    }

    public async Task<PagedResultDto<OperLogDto>> GetListAsync(OperLogGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.DbQueryable.WhereIF(!string.IsNullOrEmpty(input.OperUser),
                x => x.OperUser.Contains(input.OperUser!))
            .WhereIF(input.OperType is not null, x => x.OperType == input.OperType)
            .WhereIF(input.StartTime is not null && input.EndTime is not null,
                x => x.ExecutionTime >= input.StartTime && x.ExecutionTime <= input.EndTime)
            .ToPageListAsync(input.PageNum, input.PageSize, total);

        return new PagedResultDto<OperLogDto>(total, entities.Adapt<List<OperLogDto>>());
    }

    public async Task DeleteAsync([FromBody] IEnumerable<Guid> id)
    {
        await _repository.DeleteManyAsync(id);
    }

    public async Task<IActionResult> GetExportExcelAsync(OperLogGetListInput input)
    {
        if (input is PagedInput paged)
        {
            paged.PageNum = 0;
            paged.PageSize = int.MaxValue;
        }

        var output = await GetListAsync(input);
        return new PhysicalFileResult(ExporterHelper.ExportExcel(output.Items), "application/vnd.ms-excel");
    }
}