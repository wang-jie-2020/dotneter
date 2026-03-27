using Microsoft.AspNetCore.Mvc;
using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IRoleService
{
    Task<RoleDto> GetAsync(long id);

    Task<PagedResult<RoleDto>> GetListAsync(RoleQuery query);

    Task<RoleDto> CreateAsync(RoleInput input);

    Task<RoleDto> UpdateAsync(long id, RoleInput input);

    Task DeleteAsync(IEnumerable<long> id);
    
    Task UpdateDataScopeAsync(UpdateDataScopeInput input);

    Task<RoleDto> UpdateStateAsync(long id, bool state);

    /// <summary>
    ///     获取角色下的用户
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="query"></param>
    /// <param name="isAllocated">是否在该角色下</param>
    /// <returns></returns>
    Task<PagedResult<UserDto>> GetAuthUserByRoleIdAsync([FromRoute] long roleId,
        [FromRoute] bool isAllocated, [FromQuery] RoleAuthUserQuery query);

    /// <summary>
    ///     批量给用户授权
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task CreateAuthUserAsync(RoleAuthUserInput input);

    /// <summary>
    ///     批量取消授权
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task DeleteAuthUserAsync(RoleAuthUserInput input);
}