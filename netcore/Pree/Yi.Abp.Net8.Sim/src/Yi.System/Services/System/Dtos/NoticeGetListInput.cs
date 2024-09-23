using Yi.AspNetCore.System;
using Yi.System.Domains.System.Entities;

namespace Yi.System.Services.System.Dtos;

public class NoticeGetListInput : PagedInput
{
    public string? Title { get; set; }
    
    public NoticeTypeEnum? Type { get; set; }
}