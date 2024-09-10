using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.Infra.TenantManagement.Dtos;
using Yi.Infra.TenantManagement.Services;

namespace Yi.Infra.TenantManagement.Controllers;

[Route("api/app/tenant")]
public class TenantController : AbpController
{
    private readonly ITenantService _tenantService;

    public TenantController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpGet("{id}")]
    public async Task<TenantGetOutputDto> GetAsync([FromRoute] Guid id)
    {
        return await _tenantService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<TenantGetListOutputDto>> GetListAsync(TenantGetListInput input)
    {
        return await _tenantService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<TenantGetOutputDto> CreateAsync([FromBody] TenantCreateInput input)
    {
        return await _tenantService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<TenantGetOutputDto> UpdateAsync([FromRoute] Guid id, [FromBody] TenantUpdateInput input)
    {
        return await _tenantService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync(IEnumerable<Guid> id)
    {
        await _tenantService.DeleteAsync(id);
    }

    [HttpGet("export-excel")]
    public async Task<IActionResult> GetExportExcelAsync(TenantGetListInput input)
    {
        return await _tenantService.GetExportExcelAsync(input);
    }

    [HttpGet("select")]
    public async Task<List<TenantSelectOutputDto>> GetSelectAsync()
    {
        return await _tenantService.GetSelectAsync();
    }

    [HttpPut("init/{id}")]
    public async Task InitAsync([FromRoute] Guid id)
    {
        await _tenantService.InitAsync(id);
    }
}