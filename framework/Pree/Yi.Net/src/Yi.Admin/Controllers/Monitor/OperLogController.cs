using Microsoft.AspNetCore.Mvc;
using Yi.Framework;
using Yi.Framework.Abstractions;
using Yi.Framework.SqlSugarCore.Repositories;
using Yi.System.Entities;
using Yi.System.Services.Dtos;

namespace Yi.Web.Controllers.Monitor;

[ApiController]
[Route("api/monitor/oper-log")]
public class OperLogController : BaseController
{
    private readonly ISqlSugarRepository<OperLogEntity> _repository;
    
    public OperLogController(ISqlSugarRepository<OperLogEntity> repository)
    {
        _repository = repository;
    }

    [HttpGet("{id}")]
    public async Task<OperLogDto> GetAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<OperLogDto>();
    }

    [HttpGet]
    public async Task<PagedResult<OperLogDto>> GetListAsync([FromQuery] OperLogQuery query)
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

    [HttpDelete]
    public async Task DeleteAsync([FromBody] IEnumerable<long> id)
    {
        await _repository.DeleteByIdsAsync(id.Select(x => (object)x).ToArray());
    }
}