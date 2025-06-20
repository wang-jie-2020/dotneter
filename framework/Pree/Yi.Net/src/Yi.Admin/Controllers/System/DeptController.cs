using Microsoft.AspNetCore.Mvc;
using Yi.AspNetCore.Core;
using Yi.Framework.Abstractions;
using Yi.Framework.Core;
using Yi.Framework.Core.Abstractions;
using Yi.System.Services;
using Yi.System.Services.Dtos;

namespace Yi.Web.Controllers.System;

[ApiController]
[Route("api/system/dept")]
public class DeptController : BaseController
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
    public async Task<PagedResult<DeptGetListOutputDto>> GetListAsync([FromQuery] DeptGetListInput input)
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