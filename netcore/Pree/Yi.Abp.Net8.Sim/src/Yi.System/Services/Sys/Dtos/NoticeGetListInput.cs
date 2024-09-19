using Yi.AspNetCore.Common;
using Yi.System.Domains.Sys.Entities;

namespace Yi.System.Services.Sys.Dtos;

public class NoticeGetListInput : PagedInfraInput
{
    public string? Title { get; set; }
    
    public NoticeTypeEnum? Type { get; set; }
}