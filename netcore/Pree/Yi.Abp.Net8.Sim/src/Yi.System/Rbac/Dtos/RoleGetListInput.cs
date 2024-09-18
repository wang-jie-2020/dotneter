namespace Yi.System.Rbac.Dtos;

public class RoleGetListInput : PagedInfraInput
{
    public string? RoleName { get; set; }
    
    public string? RoleCode { get; set; }
    
    public bool? State { get; set; }
}