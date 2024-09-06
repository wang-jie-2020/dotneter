using Yi.Abp.Infra.Rbac.Enums;

namespace Yi.Abp.Infra.Rbac.Dtos.Role
{
    public class UpdateDataScpoceInput
    {
        public Guid RoleId { get; set; }

        public List<Guid>? DeptIds { get; set; }

        public DataScopeEnum DataScope { get; set; }
    }
}
