using Volo.Abp.Application.Dtos;
using Yi.Infra.Rbac.Enums;

namespace Yi.Infra.Rbac.Dtos.Role;

public class RoleGetOutputDto : EntityDto<Guid>
{
    public DateTime CreationTime { get; set; } = DateTime.Now;
    public Guid? CreatorId { get; set; }
    public string? RoleName { get; set; }
    public string? RoleCode { get; set; }
    public string? Remark { get; set; }
    public DataScopeEnum DataScope { get; set; } = DataScopeEnum.ALL;
    public bool State { get; set; }

    public int OrderNum { get; set; }
}