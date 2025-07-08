namespace Yi.System.Services.Dtos;

/// <summary>
///     配置查询参数
/// </summary>
public class ConfigQuery : PagedQuery
{
    /// <summary>
    ///     配置名称
    /// </summary>
    public string? ConfigName { get; set; }

    /// <summary>
    ///     配置键
    /// </summary>
    public string? ConfigKey { get; set; }
}