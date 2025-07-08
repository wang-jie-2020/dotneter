using Microsoft.AspNetCore.Mvc;
using Yi.Framework;
using Yi.Framework.Abstractions;
using Yi.Framework.SqlSugarCore.Repositories;
using Yi.System.Monitor.Dtos;
using Yi.System.Monitor.Entities;

namespace Yi.Web.Controllers.Monitor;

[ApiController]
[Route("api/monitor/login-log")]
public class LoginLogController : BaseController
{
    private readonly ISqlSugarRepository<LoginLogEntity> _repository;

    public LoginLogController(ISqlSugarRepository<LoginLogEntity> repository)
    {
        _repository = repository;
    }

    [HttpGet("{id}")]
    public async Task<LoginLogGetListOutputDto> GetAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity.Adapt<LoginLogGetListOutputDto>();
    }

    [HttpGet]
    public async Task<PagedResult<LoginLogGetListOutputDto>> GetListAsync([FromQuery] LoginLogGetListQueryVo query)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(query.LoginIp), x => x.LoginIp.Contains(query.LoginIp!))
            .WhereIF(!string.IsNullOrEmpty(query.LoginUser), x => x.LoginUser!.Contains(query.LoginUser!))
            .WhereIF(query.StartTime is not null && query.EndTime is not null,
                x => x.CreationTime >= query.StartTime && x.CreationTime <= query.EndTime)
            .ToPageListAsync(query.PageNum, query.PageSize, total);
        
        return new PagedResult<LoginLogGetListOutputDto>(total, entities.Adapt<List<LoginLogGetListOutputDto>>());
    }
}