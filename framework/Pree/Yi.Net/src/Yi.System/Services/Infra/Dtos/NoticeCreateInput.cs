using Yi.Sys.Domains.Infra.Entities;

namespace Yi.Sys.Services.Infra.Dtos;

public class NoticeCreateInput
{
    public string Title { get; set; }
    
    public NoticeTypeEnum Type { get; set; }
    
    public string Content { get; set; }
    
    public int OrderNum { get; set; }
    
    public bool State { get; set; }
}