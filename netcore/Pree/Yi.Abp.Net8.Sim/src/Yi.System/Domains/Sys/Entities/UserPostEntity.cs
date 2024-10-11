using Yi.AspNetCore.System.Entities;

namespace Yi.System.Domains.Sys.Entities;

/// <summary>
///     用户岗位表
/// </summary>
[SugarTable("Sys_UserPost")]
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