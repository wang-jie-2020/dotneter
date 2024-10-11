using Yi.System.Domains.Sys.Entities;

namespace Yi.System.Services.Sys.Dtos;

public class NoticeUpdateInput
{
    public string? Title { get; set; }
    
    public NoticeTypeEnum? Type { get; set; }
    
    public string? Content { get; set; }
    
    public int? OrderNum { get; set; }
    
    public bool? State { get; set; }
}