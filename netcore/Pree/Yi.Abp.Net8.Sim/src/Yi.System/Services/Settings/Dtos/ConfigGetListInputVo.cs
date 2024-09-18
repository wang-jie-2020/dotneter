using Yi.AspNetCore.Common;

namespace Yi.System.Services.Settings.Dtos;

/// <summary>
///     配置查询参数
/// </summary>
public class ConfigGetListInputVo : PagedInfraInput
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