using Volo.Abp.Application.Dtos;

namespace Yi.Abp.Infra.Rbac.Dtos.Dictionary
{
    public class DictionaryGetListInputVo : PagedAndSortedResultRequestDto
    {
        public string? DictType { get; set; }
        public string? DictLabel { get; set; }
        public bool? State { get; set; }
    }
}
