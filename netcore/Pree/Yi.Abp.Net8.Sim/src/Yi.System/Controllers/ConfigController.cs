using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.System.Services.Sys;
using Yi.System.Services.Sys.Dtos;

namespace Yi.System.Controllers;

[ApiController]
[Route("api/app/config")]
public class ConfigController : AbpController
{
    private readonly IConfigService _configService;

    public ConfigController(IConfigService configService)
    {
        _configService = configService;
    }

    [HttpGet("{id}")]
    public async Task<ConfigGetOutputDto> GetAsync(Guid id)
    {
        return await _configService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<ConfigGetListOutputDto>> GetListAsync([FromQuery] ConfigGetListInputVo input)
    {
        return await _configService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<ConfigGetOutputDto> CreateAsync([FromBody] ConfigCreateInputVo input)
    {
        return await _configService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<ConfigGetOutputDto> UpdateAsync(Guid id, [FromBody] ConfigUpdateInput input)
    {
        return await _configService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _configService.DeleteAsync(id);
    }

    [HttpGet("export-excel")]
    public async Task<IActionResult> GetExportExcelAsync([FromQuery] ConfigGetListInputVo input)
    {
        return await _configService.GetExportExcelAsync(input);
    }
}