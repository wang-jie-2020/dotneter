using Microsoft.AspNetCore.Mvc;
using Yi.Framework.Abstractions;
using Yi.System.Services;
using Yi.System.Services.Dtos;

namespace Yi.Admin.Controllers.System;

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
    public async Task<DeptDto> GetAsync(Guid id)
    {
        return await _deptService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResult<DeptDto>> GetListAsync([FromQuery] DeptQuery query)
    {
        return await _deptService.GetListAsync(query);
    }

    [HttpPost]
    public async Task<DeptDto> CreateAsync([FromBody] DeptInput input)
    {
        return await _deptService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<DeptDto> UpdateAsync(Guid id, [FromBody] DeptInput input)
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
    public async Task<List<DeptDto>> GetRoleIdAsync(Guid roleId)
    {
        return await _deptService.GetRoleIdAsync(roleId);
    }
}