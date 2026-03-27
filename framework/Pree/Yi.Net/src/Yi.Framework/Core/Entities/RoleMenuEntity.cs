using SqlSugar;
using Yi.Framework.SqlSugarCore;

namespace Yi.Framework.Core.Entities;

/// <summary>
///     角色菜单关系表
/// </summary>
[SugarTable("Sys_RoleMenu")]
public class RoleMenuEntity : Entity<long>
{
    /// <summary>
    /// </summary>
    [SugarColumn(ColumnName = "RoleId")]
    public long RoleId { get; set; }

    /// <summary>
    /// </summary>
    [SugarColumn(ColumnName = "MenuId")]
    public long MenuId { get; set; }
}