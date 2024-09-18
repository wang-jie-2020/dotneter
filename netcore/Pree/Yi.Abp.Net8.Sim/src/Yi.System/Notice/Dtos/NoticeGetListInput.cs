using Yi.System.Notice.Entities;

namespace Yi.System.Notice.Dtos;

/// <summary>
///     查询参数
/// </summary>
public class NoticeGetListInput : PagedInfraInput
{
    public string? Title { get; set; }
    
    public NoticeTypeEnum? Type { get; set; }
}