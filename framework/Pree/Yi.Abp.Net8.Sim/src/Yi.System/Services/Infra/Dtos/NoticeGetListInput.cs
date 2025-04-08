using Yi.AspNetCore.System;
using Yi.Sys.Domains.Infra.Entities;

namespace Yi.Sys.Services.Infra.Dtos;

public class NoticeGetListInput : PagedInput
{
    public string? Title { get; set; }
    
    public NoticeTypeEnum? Type { get; set; }
}