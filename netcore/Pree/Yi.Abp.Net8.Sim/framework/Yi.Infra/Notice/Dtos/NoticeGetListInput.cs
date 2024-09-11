using Yi.Infra.Notice.Entities;

namespace Yi.Infra.Notice.Dtos;

/// <summary>
///     查询参数
/// </summary>
public class NoticeGetListInput : PagedInfraInput
{
    public string? Title { get; set; }
    
    public NoticeTypeEnum? Type { get; set; }
}