using Microsoft.AspNetCore.Mvc;
using Yi.Framework.Abstractions;
using Yi.System.Monitor.Dtos;
using Yi.System.Monitor.Entities;

namespace Yi.System.Monitor.Impl;

public class OperLogService : BaseService, IOperLogService
{
    private readonly ISqlSugarRepository<OperLogEntity> _repository;

    public OperLogService(ISqlSugarRepository<OperLogEntity> repository)
    {
        _repository = repository;
    }

    public async Task<OperLogDto> GetAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<OperLogDto>();
    }

    public async Task<PagedResult<OperLogDto>> GetListAsync(OperLogGetListQuery query)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.AsQueryable().WhereIF(!string.IsNullOrEmpty(query.OperUser),
                x => x.OperUser.Contains(query.OperUser!))
            .WhereIF(query.OperType is not null, x => x.OperType == query.OperType)
            .WhereIF(query.StartTime is not null && query.EndTime is not null,
                x => x.ExecutionTime >= query.StartTime && x.ExecutionTime <= query.EndTime)
            .ToPageListAsync(query.PageNum, query.PageSize, total);

        return new PagedResult<OperLogDto>(total, entities.Adapt<List<OperLogDto>>());
    }

    public async Task DeleteAsync([FromBody] IEnumerable<long> id)
    {
        await _repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }
}