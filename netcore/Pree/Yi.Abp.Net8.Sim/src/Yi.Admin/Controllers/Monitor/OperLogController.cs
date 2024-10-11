using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.Sys.Services.Monitor;
using Yi.Sys.Services.Monitor.Dtos;

namespace Yi.Admin.Controllers.Monitor;

[ApiController]
[Route("api/monitor/oper-log")]
public class OperLogController : AbpController
{
    private readonly IOperLogService _operLogService;

    public OperLogController(IOperLogService operLogService)
    {
        _operLogService = operLogService;
    }

    [HttpGet("{id}")]
    public async Task<OperLogDto> GetAsync(Guid id)
    {
        return await _operLogService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<OperLogDto>> GetListAsync([FromQuery] OperLogGetListInput input)
    {
        return await _operLogService.GetListAsync(input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromBody] IEnumerable<Guid> id)
    {
        await _operLogService.DeleteAsync(id);
    }
}