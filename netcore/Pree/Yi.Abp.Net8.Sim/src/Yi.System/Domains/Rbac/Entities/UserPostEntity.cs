using Volo.Abp.Domain.Entities;
using Yi.AspNetCore.SqlSugarCore.Entities;

namespace Yi.System.Domains.Rbac.Entities;

/// <summary>
///     用户岗位表
/// </summary>
[SugarTable("UserPost")]
public class UserPostEntity : SimpleEntity<Guid>
{
    /// <summary>
    ///     用户id
    /// </summary>
    [SugarColumn(ColumnName = "UserId")]
    public Guid UserId { get; set; }

    /// <summary>
    ///     岗位id
    /// </summary>
    [SugarColumn(ColumnName = "PostId")]
    public Guid PostId { get; set; }
}