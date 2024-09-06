using Yi.Abp.Infra.Rbac.Enums;

namespace Yi.Abp.Infra.Rbac.Dtos.Role
{
    public class RoleUpdateInputVo
    {
        public string? RoleName { get; set; }
        public string? RoleCode { get; set; }
        public string? Remark { get; set; }
        public DataScopeEnum DataScope { get; set; } = DataScopeEnum.ALL;
        public bool State { get; set; }

        public int OrderNum { get; set; }

        public List<Guid>? DeptIds { get; set; }

        public List<Guid>? MenuIds { get; set; }
    }
}
