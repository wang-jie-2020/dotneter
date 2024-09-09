using Yi.Infra.Rbac.Enums;

namespace Yi.Infra.Rbac.Model;

public class RoleTokenInfoModel
{
    public Guid Id { get; set; }
    public DataScopeEnum DataScope { get; set; }
}