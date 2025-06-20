using Microsoft.AspNetCore.Mvc;
using Yi.AspNetCore.Core;
using Yi.Framework.Core;
using Yi.Framework.Core.Abstractions;
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
    public async Task<PagedResult<TenantDto>> GetListAsync([FromQuery] TenantGetListInput input)
    {
        return await _tenantService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<TenantDto> CreateAsync([FromBody] TenantCreateInput input)
    {
        return await _tenantService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<TenantDto> UpdateAsync(Guid id, [FromBody] TenantUpdateInput input)
    {
        return await _tenantService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _tenantService.DeleteAsync(id);
    }

    [HttpGet("export")]
    public async Task<IActionResult> GetExportExcelAsync([FromQuery] TenantGetListInput input)
    {
        return await _tenantService.GetExportExcelAsync(input);
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