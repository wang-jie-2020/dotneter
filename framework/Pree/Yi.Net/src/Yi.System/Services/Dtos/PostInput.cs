namespace Yi.System.Services.Dtos;

/// <summary>
///     Post输入创建对象
/// </summary>
public class PostInput
{
    public bool? State { get; set; }
    
    public int OrderNum { get; set; }
    
    public string PostCode { get; set; } = string.Empty;
    
    public string PostName { get; set; } = string.Empty;
    
    public string? Remark { get; set; }
}