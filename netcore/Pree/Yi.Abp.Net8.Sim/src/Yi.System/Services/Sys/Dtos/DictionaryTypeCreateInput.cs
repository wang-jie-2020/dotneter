namespace Yi.System.Services.Sys.Dtos;

/// <summary>
///     DictionaryType输入创建对象
/// </summary>
public class DictionaryTypeCreateInput
{
    public string DictName { get; set; } = string.Empty;
    
    public string DictType { get; set; } = string.Empty;
    
    public string? Remark { get; set; }

    public bool State { get; set; }
}