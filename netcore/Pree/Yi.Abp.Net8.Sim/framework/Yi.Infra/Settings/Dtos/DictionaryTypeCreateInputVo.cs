namespace Yi.Infra.Settings.Dtos;

/// <summary>
///     DictionaryType输入创建对象
/// </summary>
public class DictionaryTypeCreateInputVo
{
    public string DictName { get; set; } = string.Empty;
    
    public string DictType { get; set; } = string.Empty;
    
    public string? Remark { get; set; }

    public bool State { get; set; }
}