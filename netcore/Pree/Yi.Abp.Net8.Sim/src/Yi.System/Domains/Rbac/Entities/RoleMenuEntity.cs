using Volo.Abp.Domain.Entities;

namespace Yi.System.Domains.Rbac.Entities;

/// <summary>
///     角色菜单关系表
/// </summary>
[SugarTable("RoleMenu")]
public class RoleMenuEntity : Entity<Guid>
{
    /// <summary>
    ///     主键
    /// </summary>
    [SugarColumn(IsPrimaryKey = true)]
    public override Guid Id { get; protected set; }

    /// <summary>
    /// </summary>
    [SugarColumn(ColumnName = "RoleId")]
    public Guid RoleId { get; set; }

    /// <summary>
    /// </summary>
    [SugarColumn(ColumnName = "MenuId")]
    public Guid MenuId { get; set; }
}