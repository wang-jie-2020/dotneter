using Yi.AspNetCore.Core.Entities;

namespace Yi.Sys.Domains.Infra.Entities;

/// <summary>
///     角色部门关系表
/// </summary>
[SugarTable("Sys_RoleDept")]
public class RoleDeptEntity : SimpleEntity<long>
{
    /// <summary>
    ///     角色id
    /// </summary>
    [SugarColumn(ColumnName = "RoleId")]
    public Guid RoleId { get; set; }

    /// <summary>
    ///     部门id
    /// </summary>
    [SugarColumn(ColumnName = "DeptId")]
    public Guid DeptId { get; set; }
}