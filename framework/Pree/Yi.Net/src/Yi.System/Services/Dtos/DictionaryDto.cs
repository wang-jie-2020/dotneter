namespace Yi.System.Services.Dtos;

public class DictionaryDto 
{
    public Guid Id { get; set; }
    
    public DateTime CreationTime { get; set; } = DateTime.Now;
    
    public Guid? CreatorId { get; set; }
    
    public string? Remark { get; set; }
    
    public string? ListClass { get; set; }
    
    public string? CssClass { get; set; }
    
    public string DictType { get; set; } = string.Empty;
    
    public string? DictLabel { get; set; }
    
    public string DictValue { get; set; } = string.Empty;
    
    public bool IsDefault { get; set; }

    public bool State { get; set; }
    
    public int OrderNum { get; set; } = 0;
}