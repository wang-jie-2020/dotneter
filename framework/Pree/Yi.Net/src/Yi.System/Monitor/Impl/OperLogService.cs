using Microsoft.AspNetCore.Mvc;
using Yi.Framework.Core;
using Yi.Framework.Core.Abstractions;
using Yi.Framework.SqlSugarCore.Repositories;
using Yi.System.Monitor.Dtos;
using Yi.System.Monitor.Entities;

namespace Yi.System.Monitor.Impl;

public class OperLogService : BaseService, IOperLogService
{
    private readonly ISqlSugarRepository<OperLogEntity, long> _repository;

    public OperLogService(ISqlSugarRepository<OperLogEntity, long> repository)
    {
        _repository = repository;
    }

    public async Task<OperLogDto> GetAsync(long id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<OperLogDto>();
    }

    public async Task<PagedResult<OperLogDto>> GetListAsync(OperLogGetListInput input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.DbQueryable.WhereIF(!string.IsNullOrEmpty(input.OperUser),
                x => x.OperUser.Contains(input.OperUser!))
            .WhereIF(input.OperType is not null, x => x.OperType == input.OperType)
            .WhereIF(input.StartTime is not null && input.EndTime is not null,
                x => x.ExecutionTime >= input.StartTime && x.ExecutionTime <= input.EndTime)
            .ToPageListAsync(input.PageNum, input.PageSize, total);

        return new PagedResult<OperLogDto>(total, entities.Adapt<List<OperLogDto>>());
    }

    public async Task DeleteAsync([FromBody] IEnumerable<long> id)
    {
        await _repository.DeleteManyAsync(id);
    }
}