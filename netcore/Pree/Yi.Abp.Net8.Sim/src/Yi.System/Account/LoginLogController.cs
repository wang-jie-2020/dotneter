using Mapster;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.AspNetCore.SqlSugarCore;
using Yi.System.Account.Dtos;

namespace Yi.System.Account;

[ApiController]
[Route("api/app/login-log")]
public class LoginLogController : AbpController
{
    private readonly ISqlSugarRepository<LoginLogAggregateRoot, Guid> _repository;

    public LoginLogController(ISqlSugarRepository<LoginLogAggregateRoot, Guid> repository)
    {
        _repository = repository;
    }

    [HttpGet("{id}")]
    public async Task<LoginLogGetListOutputDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        return entity.Adapt<LoginLogGetListOutputDto>();
    }

    [HttpGet]
    public async Task<PagedResultDto<LoginLogGetListOutputDto>> GetListAsync([FromQuery] LoginLogGetListInputVo input)
    {
        RefAsync<int> total = 0;

        var entities = await _repository._DbQueryable
            .WhereIF(!string.IsNullOrEmpty(input.LoginIp), x => x.LoginIp.Contains(input.LoginIp!))
            .WhereIF(!string.IsNullOrEmpty(input.LoginUser), x => x.LoginUser!.Contains(input.LoginUser!))
            .WhereIF(input.StartTime is not null && input.EndTime is not null,
                x => x.CreationTime >= input.StartTime && x.CreationTime <= input.EndTime)
            .ToPageListAsync(input.SkipCount, input.MaxResultCount, total);
        return new PagedResultDto<LoginLogGetListOutputDto>(total, entities.Adapt<List<LoginLogGetListOutputDto>>());
    }
}