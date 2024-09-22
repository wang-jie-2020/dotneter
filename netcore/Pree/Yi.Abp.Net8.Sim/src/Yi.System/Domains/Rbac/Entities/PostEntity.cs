using Yi.AspNetCore.SqlSugarCore.Entities;

namespace Yi.System.Domains.Rbac.Entities;

/// <summary>
///     岗位表
/// </summary>
[SugarTable("Post")]
public class PostEntity : BizEntity<Guid>
{
    /// <summary>
    ///     岗位编码
    /// </summary>
    [SugarColumn(ColumnName = "PostCode")]
    public string PostCode { get; set; } = string.Empty;

    /// <summary>
    ///     岗位名称
    /// </summary>
    [SugarColumn(ColumnName = "PostName")]
    public string PostName { get; set; } = string.Empty;

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