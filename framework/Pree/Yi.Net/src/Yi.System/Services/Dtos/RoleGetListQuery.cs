namespace Yi.System.Services.Dtos;

public class RoleGetListQuery : PagedQuery
{
    public string? RoleName { get; set; }
    
    public string? RoleCode { get; set; }
    
    public bool? State { get; set; }
}