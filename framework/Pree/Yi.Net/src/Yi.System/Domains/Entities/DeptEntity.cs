using Yi.AspNetCore.Core.Entities;

namespace Yi.System.Domains.Entities;

/// <summary>
///     部门表
/// </summary>
[SugarTable("Sys_Dept")]
public class DeptEntity : BizEntity<Guid>
{
    public DeptEntity()
    {
    }

    public DeptEntity(Guid id)
    {
        this.Id = id;
        ParentId = Guid.Empty;
    }

    public DeptEntity(Guid id, Guid parentId)
    {
        this.Id = id;
        ParentId = parentId;
    }

    /// <summary>
    ///     部门名称
    /// </summary>
    public string DeptName { get; set; } = string.Empty;

    /// <summary>
    ///     部门编码
    /// </summary>
    [SugarColumn(ColumnName = "DeptCode")]
    public string DeptCode { get; set; } = string.Empty;

    /// <summary>
    ///     负责人
    /// </summary>
    [SugarColumn(ColumnName = "Leader")]
    public string? Leader { get; set; }

    /// <summary>
    ///     父级id
    /// </summary>
    [SugarColumn(ColumnName = "ParentId")]
    public Guid ParentId { get; set; }

    /// <summary>
    ///     描述
    /// </summary>
    [SugarColumn(ColumnName = "Remark")]
    public string? Remark { get; set; }

    /// <summary>
    ///     排序
    /// </summary>
    public int OrderNum { get; set; } = 0;

    /// <summary>
    ///     状态
    /// </summary>
    public bool State { get; set; } = true;
}