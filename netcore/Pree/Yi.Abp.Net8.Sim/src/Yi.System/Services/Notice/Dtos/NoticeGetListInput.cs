using Yi.AspNetCore.Common;
using Yi.System.Services.Notice.Entities;

namespace Yi.System.Services.Notice.Dtos;

/// <summary>
///     查询参数
/// </summary>
public class NoticeGetListInput : PagedInfraInput
{
    public string? Title { get; set; }
    
    public NoticeTypeEnum? Type { get; set; }
}