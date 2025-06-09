using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Yi.System.Services.Dtos;

namespace Yi.System.Services;

public interface IRoleService
{
    Task<RoleDto> GetAsync(Guid id);

    Task<PagedResultDto<RoleDto>> GetListAsync(RoleGetListInput input);

    Task<RoleDto> CreateAsync(RoleCreateInput input);

    Task<RoleDto> UpdateAsync(Guid id, RoleUpdateInput input);

    Task DeleteAsync(IEnumerable<Guid> id);
    
    Task UpdateDataScopeAsync(UpdateDataScopeInput input);

    Task<RoleDto> UpdateStateAsync(Guid id, bool state);

    /// <summary>
    ///     获取角色下的用户
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="input"></param>
    /// <param name="isAllocated">是否在该角色下</param>
    /// <returns></returns>
    Task<PagedResultDto<UserGetListOutputDto>> GetAuthUserByRoleIdAsync([FromRoute] Guid roleId,
        [FromRoute] bool isAllocated, [FromQuery] RoleAuthUserGetListInput input);

    /// <summary>
    ///     批量给用户授权
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task CreateAuthUserAsync(RoleAuthUserCreateOrDeleteInput input);

    /// <summary>
    ///     批量取消授权
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task DeleteAuthUserAsync(RoleAuthUserCreateOrDeleteInput input);
}