namespace Yi.System.Settings.Dtos;

public class ConfigUpdateInput
{
    public Guid Id { get; set; }
    
    public string ConfigName { get; set; } = string.Empty;
    
    public string ConfigKey { get; set; } = string.Empty;
    
    public string ConfigValue { get; set; } = string.Empty;
    
    public string? ConfigType { get; set; }
    
    public int OrderNum { get; set; }
    
    public string? Remark { get; set; }
    
    public DateTime CreationTime { get; set; }
}