using Yi.Framework.Ddd.Application;

namespace Yi.Infra.Rbac.Dtos.Config;

/// <summary>
///     配置查询参数
/// </summary>
public class ConfigGetListInputVo : PagedAllResultRequestDto
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