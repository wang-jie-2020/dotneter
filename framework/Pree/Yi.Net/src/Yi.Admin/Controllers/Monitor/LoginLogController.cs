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
    private readonly ISqlSugarRepository<LoginLogEntity, long> _repository;

    public LoginLogController(ISqlSugarRepository<LoginLogEntity, long> repository)
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
    public async Task<PagedResult<LoginLogGetListOutputDto>> GetListAsync([FromQuery] LoginLogGetListInputVo input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(input.LoginIp), x => x.LoginIp.Contains(input.LoginIp!))
            .WhereIF(!string.IsNullOrEmpty(input.LoginUser), x => x.LoginUser!.Contains(input.LoginUser!))
            .WhereIF(input.StartTime is not null && input.EndTime is not null,
                x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.PageNum, input.PageSize, total);
        
        return new PagedResult<LoginLogGetListOutputDto>(total, entities.Adapt<List<LoginLogGetListOutputDto>>());
    }
}