namespace Yi.System.Services.Dtos;

public class DictionaryTypeInput
{
    public string DictName { get; set; } = string.Empty;
    
    public string DictType { get; set; } = string.Empty;
    
    public string? Remark { get; set; }
    
    public bool State { get; set; }
}