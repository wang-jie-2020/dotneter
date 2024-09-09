using Yi.Infra.Rbac.Consts;

namespace Yi.Infra.Rbac.Dtos.Notice;

/// <summary>
///     查询参数
/// </summary>
public class NoticeGetListInput : PagedInfraInput
{
    public string? Title { get; set; }
    public NoticeTypeEnum? Type { get; set; }
}