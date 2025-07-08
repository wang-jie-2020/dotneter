using Yi.System.Entities;

namespace Yi.System.Services.Dtos;

public class NoticeInput
{
    public string Title { get; set; }
    
    public NoticeTypeEnum Type { get; set; }
    
    public string Content { get; set; }
    
    public int OrderNum { get; set; }
    
    public bool State { get; set; }
}