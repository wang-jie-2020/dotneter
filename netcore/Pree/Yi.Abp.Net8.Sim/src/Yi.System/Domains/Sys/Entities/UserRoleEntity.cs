using Yi.AspNetCore.System.Entities;

namespace Yi.System.Domains.Sys.Entities;

/// <summary>
///     用户角色关系表
/// </summary>
[SugarTable("Sys_UserRole")]
public class UserRoleEntity : SimpleEntity<Guid>
{
    /// <summary>
    ///     角色id
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    ///     用户id
    /// </summary>
    public Guid UserId { get; set; }
}