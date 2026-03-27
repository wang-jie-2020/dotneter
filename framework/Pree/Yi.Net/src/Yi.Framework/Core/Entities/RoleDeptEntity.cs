using SqlSugar;
using Yi.Framework.SqlSugarCore;

namespace Yi.Framework.Core.Entities;

/// <summary>
///     角色部门关系表
/// </summary>
[SugarTable("Sys_RoleDept")]
public class RoleDeptEntity : Entity<long>
{
    /// <summary>
    ///     角色id
    /// </summary>
    [SugarColumn(ColumnName = "RoleId")]
    public long RoleId { get; set; }

    /// <summary>
    ///     部门id
    /// </summary>
    [SugarColumn(ColumnName = "DeptId")]
    public long DeptId { get; set; }
}