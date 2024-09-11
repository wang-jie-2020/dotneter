namespace Yi.Infra.Settings.Dtos;

public class DictionaryTypeGetListInputVo : PagedInfraInput
{
    public string? DictName { get; set; }
    
    public string? DictType { get; set; }
    
    public string? Remark { get; set; }

    public bool? State { get; set; }
}