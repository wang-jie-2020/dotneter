namespace Yi.Infra.Rbac.Dtos.Role;

public class RoleGetListInputVo : PagedInfraInput
{
    public string? RoleName { get; set; }
    public string? RoleCode { get; set; }
    public bool? State { get; set; }
}