namespace Yi.Sys.Services.Infra.Dtos;

public class DictionaryTypeDto 
{
    public Guid Id { get; set; }
    
    public DateTime CreationTime { get; set; } = DateTime.Now;
    
    public Guid? CreatorId { get; set; }
    
    public string DictName { get; set; } = string.Empty;
    
    public string DictType { get; set; } = string.Empty;
    
    public string? Remark { get; set; }

    public bool State { get; set; }
}