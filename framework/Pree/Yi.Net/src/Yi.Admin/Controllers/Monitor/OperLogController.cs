using Microsoft.AspNetCore.Mvc;
using Yi.Framework;
using Yi.Framework.Abstractions;
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
    public async Task<PagedResult<OperLogDto>> GetListAsync([FromQuery] OperLogGetListQuery query)
    {
        return await _operLogService.GetListAsync(query);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromBody] IEnumerable<long> id)
    {
        await _operLogService.DeleteAsync(id);
    }
}