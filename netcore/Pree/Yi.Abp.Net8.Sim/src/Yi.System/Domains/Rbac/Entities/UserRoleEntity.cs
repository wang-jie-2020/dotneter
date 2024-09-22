using Yi.AspNetCore.SqlSugarCore.Entities;

namespace Yi.System.Domains.Rbac.Entities;

/// <summary>
///     用户角色关系表
/// </summary>
[SugarTable("UserRole")]
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