using Yi.AspNetCore.System.Entities;

namespace Yi.System.Domains.Rbac.Entities;

/// <summary>
///     角色菜单关系表
/// </summary>
[SugarTable("RoleMenu")]
public class RoleMenuEntity : SimpleEntity<Guid>
{
    /// <summary>
    /// </summary>
    [SugarColumn(ColumnName = "RoleId")]
    public Guid RoleId { get; set; }

    /// <summary>
    /// </summary>
    [SugarColumn(ColumnName = "MenuId")]
    public Guid MenuId { get; set; }
}