namespace Yi.System.Services.Dtos;

/// <summary>
///     Config输入创建对象
/// </summary>
public class ConfigCreateInputVo
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