using Yi.AspNetCore.Core;
using Yi.Framework.Core;
using Yi.System.Domains.Entities;

namespace Yi.System.Services.Dtos;

public class NoticeGetListInput : PagedInput
{
    public string? Title { get; set; }
    
    public NoticeTypeEnum? Type { get; set; }
}