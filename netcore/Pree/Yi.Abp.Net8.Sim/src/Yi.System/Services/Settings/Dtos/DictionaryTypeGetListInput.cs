namespace Yi.System.Services.Settings.Dtos;

public class DictionaryTypeGetListInput : PagedInfraInput
{
    public string? DictName { get; set; }
    
    public string? DictType { get; set; }
    
    public string? Remark { get; set; }

    public bool? State { get; set; }
}