using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.Infra.TenantManagement.Dtos;
using Yi.Infra.TenantManagement.Services;

namespace Yi.Infra.TenantManagement.Controllers;

[ApiController]
[Route("api/app/tenant")]
public class TenantController : AbpController
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
    public async Task<PagedResultDto<TenantDto>> GetListAsync([FromQuery] TenantGetListInput input)
    {
        return await _tenantService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<TenantDto> CreateAsync(TenantCreateInput input)
    {
        return await _tenantService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<TenantDto> UpdateAsync(Guid id, TenantUpdateInput input)
    {
        return await _tenantService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _tenantService.DeleteAsync(id);
    }

    [HttpGet("export-excel")]
    public async Task<IActionResult> GetExportExcelAsync(TenantGetListInput input)
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