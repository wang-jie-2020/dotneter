using Yi.System.Domains.Entities;

namespace Yi.System.Services.Dtos;

public class NoticeDto
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public NoticeTypeEnum Type { get; set; }
    
    public string Content { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public DateTime CreationTime { get; set; }
    
    public Guid? CreatorId { get; set; }
    
    public Guid? LastModifierId { get; set; }
    
    public DateTime? LastModificationTime { get; set; }
    
    public int OrderNum { get; set; }
    
    public bool State { get; set; }
}