using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Yi.AspNetCore.SqlSugarCore.Entities;

namespace Yi.System.Domains.Rbac.Entities;

/// <summary>
///     角色表
/// </summary>
[SugarTable("Role")]
public class RoleEntity : BizEntity<Guid>
{
    /// <summary>
    ///     角色名
    /// </summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    ///     角色编码
    /// </summary>
    [SugarColumn(ColumnName = "RoleCode")]
    public string RoleCode { get; set; } = string.Empty;

    /// <summary>
    ///     描述
    /// </summary>
    [SugarColumn(ColumnName = "Remark")]
    public string? Remark { get; set; }

    /// <summary>
    ///     角色数据范围
    /// </summary>
    [SugarColumn(ColumnName = "DataScope")]
    public DataScopeEnum DataScope { get; set; } = DataScopeEnum.ALL;
    
    [Navigate(typeof(RoleMenuEntity), nameof(RoleMenuEntity.RoleId), nameof(RoleMenuEntity.MenuId))]
    public List<MenuEntity>? Menus { get; set; }

    [Navigate(typeof(RoleDeptEntity), nameof(RoleDeptEntity.RoleId), nameof(RoleDeptEntity.DeptId))]
    public List<DeptEntity>? Depts { get; set; }
    
    /// <summary>
    ///     排序
    /// </summary>
    public int OrderNum { get; set; } = 0;
    
    /// <summary>
    ///     状态
    /// </summary>
    public bool State { get; set; } = true;
}