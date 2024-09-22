using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.Admin.Services.Monitor;
using Yi.Admin.Services.Monitor.Dtos;

namespace Yi.Admin.Controllers;

[ApiController]
[Route("api/operation-log")]
public class OperationLogController : AbpController
{
    private readonly IOperationLogService _operationLogService;

    public OperationLogController(IOperationLogService operationLogService)
    {
        _operationLogService = operationLogService;
    }

    [HttpGet("{id}")]
    public async Task<OperationLogDto> GetAsync(Guid id)
    {
        return await _operationLogService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<OperationLogDto>> GetListAsync([FromQuery] OperationLogGetListInput input)
    {
        return await _operationLogService.GetListAsync(input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromBody] IEnumerable<Guid> id)
    {
        await _operationLogService.DeleteAsync(id);
    }

    [HttpGet("export-excel")]
    public async Task<IActionResult> GetExportExcelAsync([FromQuery] OperationLogGetListInput input)
    {
        return await _operationLogService.GetExportExcelAsync(input);
    }
}