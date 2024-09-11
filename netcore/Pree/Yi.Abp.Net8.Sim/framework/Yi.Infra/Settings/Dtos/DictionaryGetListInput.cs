using Volo.Abp.Application.Dtos;

namespace Yi.Infra.Settings.Dtos;

public class DictionaryGetListInput : PagedAndSortedResultRequestDto
{
    public string? DictType { get; set; }
    
    public string? DictLabel { get; set; }
    
    public bool? State { get; set; }
}