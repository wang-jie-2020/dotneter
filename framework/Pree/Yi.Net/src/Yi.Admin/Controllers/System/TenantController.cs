using Microsoft.AspNetCore.Mvc;
using Yi.Framework;
using Yi.Framework.Abstractions;
using Yi.System.Services;
using Yi.System.Services.Dtos;

namespace Yi.Web.Controllers.System;

[ApiController]
[Route("api/system/tenant")]
public class TenantController : BaseController
{
    private readonly ITenantService _tenantService;

    public TenantController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpGet("{id}")]
    public async Task<TenantDto> GetAsync(Guid id)
    {
        return await _tenantService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResult<TenantDto>> GetListAsync([FromQuery] TenantQuery query)
    {
        return await _tenantService.GetListAsync(query);
    }

    [HttpPost]
    public async Task<TenantDto> CreateAsync([FromBody] TenantInput input)
    {
        return await _tenantService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<TenantDto> UpdateAsync(Guid id, [FromBody] TenantInput input)
    {
        return await _tenantService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _tenantService.DeleteAsync(id);
    }

    [HttpGet("export")]
    public async Task<IActionResult> GetExportExcelAsync([FromQuery] TenantQuery query)
    {
        return await _tenantService.GetExportExcelAsync(query);
    }

    [HttpGet("select")]
    public async Task<List<TenantSelectDto>> GetSelectAsync()
    {
        return await _tenantService.GetSelectAsync();
    }

    [HttpPut("init/{id}")]
    public async Task InitAsync(Guid id)
    {
        await _tenantService.InitAsync(id);
    }
}