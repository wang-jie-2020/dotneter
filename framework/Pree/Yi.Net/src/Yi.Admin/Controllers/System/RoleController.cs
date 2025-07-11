using Microsoft.AspNetCore.Mvc;
using Yi.Framework.Abstractions;
using Yi.System.Services;
using Yi.System.Services.Dtos;

namespace Yi.Admin.Controllers.System;

[ApiController]
[Route("api/system/role")]
public class RoleController : BaseController
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("{id}")]
    public async Task<RoleDto> GetAsync(Guid id)
    {
        return await _roleService.GetAsync(id);
    }

    [HttpGet]
    public async Task<PagedResult<RoleDto>> GetListAsync([FromQuery] RoleQuery query)
    {
        return await _roleService.GetListAsync(query);
    }

    [HttpPost]
    public async Task<RoleDto> CreateAsync([FromBody] RoleInput input)
    {
        return await _roleService.CreateAsync(input);
    }

    [HttpPut("{id}")]
    public async Task<RoleDto> UpdateAsync(Guid id, [FromBody] RoleInput input)
    {
        return await _roleService.UpdateAsync(id, input);
    }

    [HttpDelete]
    public async Task DeleteAsync([FromQuery] IEnumerable<Guid> id)
    {
        await _roleService.DeleteAsync(id);
    }
    
    [HttpPut("data-scope")]
    public async Task UpdateDataScopeAsync(UpdateDataScopeInput input)
    {
        await _roleService.UpdateDataScopeAsync(input);
    }

    /// <summary>
    ///     更新状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    [HttpPut("{id}/{state}")]
    public async Task<RoleDto> UpdateStateAsync([FromRoute] Guid id, [FromRoute] bool state)
    {
        return await _roleService.UpdateStateAsync(id, state);
    }

    /// <summary>
    ///     获取角色下的用户
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="query"></param>
    /// <param name="isAllocated">是否在该角色下</param>
    /// <returns></returns>
    [HttpGet("auth-user/{roleId}/{isAllocated}")]
    public async Task<PagedResult<UserDto>> GetAuthUserByRoleIdAsync([FromRoute] Guid roleId, [FromRoute] bool isAllocated, [FromQuery] RoleAuthUserQuery query)
    {
        return await _roleService.GetAuthUserByRoleIdAsync(roleId, isAllocated, query);
    }

    /// <summary>
    ///     批量给用户授权
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("auth-user")]
    public async Task CreateAuthUserAsync([FromBody] RoleAuthUserInput input)
    {
        await _roleService.CreateAuthUserAsync(input);
    }

    /// <summary>
    ///     批量取消授权
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpDelete("auth-user")]
    public async Task DeleteAuthUserAsync([FromBody] RoleAuthUserInput input)
    {
        await _roleService.DeleteAuthUserAsync(input);
    }
}