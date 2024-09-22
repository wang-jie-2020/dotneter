using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Yi.System.Services.Rbac;
using Yi.System.Services.Rbac.Dtos;

namespace Yi.System.Controllers;

[ApiController]
[Route("api/dept")]
public class DeptController : AbpController
{
    private readonly IDeptService _deptService;

    public DeptController(IDeptService deptService)
    {
        _deptService = deptService;
    }

    [HttpGet("{id}")]
    public async Task<DeptGetOutputDto> GetAsync(Guid id)
    {
        return await _deptService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResultDto<DeptGetListOutputDto>> GetListAsync([FromQuery] DeptGetListInput input)
    {
        return await _deptService.GetListAsync(input);
    }

    [HttpPost]
    public async Task<DeptGetOutputDto> CreateAsync([FromBody] DeptCreateInput input)
    {
        return await _deptService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<DeptGetOutputDto> UpdateAsync(Guid id, [FromBody] DeptUpdateInput input)
    {
        return await _deptService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _deptService.DeleteAsync(id);
    }

    [HttpGet("export-excel")]
    public async Task<IActionResult> GetExportExcelAsync([FromQuery] DeptGetListInput input)
    {
        return await _deptService.GetExportExcelAsync(input);
    }

    /// <summary>
    ///     通过角色id查询该角色全部部门
    /// </summary>
    /// <returns></returns>
    [HttpGet("role-id/{roleId}")]
    public async Task<List<DeptGetListOutputDto>> GetRoleIdAsync(Guid roleId)
    {
        return await _deptService.GetRoleIdAsync(roleId);
    }
}