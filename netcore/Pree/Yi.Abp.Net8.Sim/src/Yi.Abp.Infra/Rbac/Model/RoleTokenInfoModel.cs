using Yi.Abp.Infra.Rbac.Enums;

namespace Yi.Abp.Infra.Rbac.Model
{
    public class RoleTokenInfoModel
    {
        public Guid Id { get; set; }
        public DataScopeEnum DataScope { get; set; }
    }
}
