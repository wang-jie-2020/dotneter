using Volo.Abp.Domain.Entities;
using Yi.AspNetCore.SqlSugarCore.Entities;

namespace Yi.System.Domains.Rbac.Entities;

/// <summary>
///     角色部门关系表
/// </summary>
[SugarTable("RoleDept")]
public class RoleDeptEntity : SimpleEntity<Guid>
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