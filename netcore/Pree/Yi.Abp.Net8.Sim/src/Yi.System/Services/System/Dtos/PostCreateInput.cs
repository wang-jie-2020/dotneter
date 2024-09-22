namespace Yi.System.Services.System.Dtos;

/// <summary>
///     Post输入创建对象
/// </summary>
public class PostCreateInput
{
    public Guid Id { get; set; }
    
    public DateTime CreationTime { get; set; } = DateTime.Now;
    
    public long? CreatorId { get; set; }
    
    public bool? State { get; set; }
    
    public int OrderNum { get; set; }
    
    public string PostCode { get; set; } = string.Empty;
    
    public string PostName { get; set; } = string.Empty;
    
    public string? Remark { get; set; }
}