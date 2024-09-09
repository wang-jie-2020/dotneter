using Yi.Framework.Ddd.Application;

namespace Yi.Infra.Rbac.Dtos.Role;

public class RoleGetListInputVo : PagedAllResultRequestDto
{
    public string? RoleName { get; set; }
    public string? RoleCode { get; set; }
    public bool? State { get; set; }
}