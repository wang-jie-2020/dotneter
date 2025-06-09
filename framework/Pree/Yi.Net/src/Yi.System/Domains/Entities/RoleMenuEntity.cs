using Yi.AspNetCore.Core.Entities;

namespace Yi.System.Domains.Entities;

/// <summary>
///     角色菜单关系表
/// </summary>
[SugarTable("Sys_RoleMenu")]
public class RoleMenuEntity : SimpleEntity<long>
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