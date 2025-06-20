using Yi.Framework.Core;

namespace Yi.System.Services.Dtos;

/// <summary>
///     配置查询参数
/// </summary>
public class ConfigGetListInputVo : PagedInput
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