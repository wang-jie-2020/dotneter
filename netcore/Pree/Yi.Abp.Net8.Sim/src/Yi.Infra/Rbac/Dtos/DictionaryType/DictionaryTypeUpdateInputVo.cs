namespace Yi.Abp.Infra.Rbac.Dtos.DictionaryType
{
    public class DictionaryTypeUpdateInputVo
    {
        public string DictName { get; set; } = string.Empty;
        public string DictType { get; set; } = string.Empty;
        public string? Remark { get; set; }
        public bool State { get; set; }
    }
}
