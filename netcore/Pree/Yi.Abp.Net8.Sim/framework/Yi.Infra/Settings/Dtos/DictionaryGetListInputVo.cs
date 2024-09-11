using Volo.Abp.Application.Dtos;

namespace Yi.Infra.Settings.Dtos;

public class DictionaryGetListInputVo : PagedAndSortedResultRequestDto
{
    public string? DictType { get; set; }
    
    public string? DictLabel { get; set; }
    
    public bool? State { get; set; }
}