using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Yi.AspNetCore.Core;
using Yi.System.Monitor;
using Yi.System.Monitor.Dtos;

namespace Yi.Web.Controllers.Monitor;

[ApiController]
[Route("api/monitor/oper-log")]
public class OperLogController : BaseController
{
    private readonly IOperLogService _operLogService;

    public OperLogController(IOperLogService operLogService)
    {
        _operLogService = operLogService;
    }

    [HttpGet("{id}")]
    public async Task<OperLogDto> GetAsync(long id)
    {
        return await _operLogService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResult<OperLogDto>> GetListAsync([FromQuery] OperLogGetListInput input)
    {
        return await _operLogService.GetListAsync(input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromBody] IEnumerable<long> id)
    {
        await _operLogService.DeleteAsync(id);
    }
}