using Yi.System.Entities;

namespace Yi.System.Services.Dtos;

public class NoticeQuery : PagedQuery
{
    public string? Title { get; set; }
    
    public NoticeTypeEnum? Type { get; set; }
}