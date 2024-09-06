using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Infra.Rbac.Dtos.DictionaryType
{
    public class DictionaryTypeGetListInputVo : PagedAllResultRequestDto
    {
        public string? DictName { get; set; }
        public string? DictType { get; set; }
        public string? Remark { get; set; }

        public bool? State { get; set; }
    }
}
