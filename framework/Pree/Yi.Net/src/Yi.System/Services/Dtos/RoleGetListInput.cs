using Yi.AspNetCore.Core;

namespace Yi.System.Services.Dtos;

public class RoleGetListInput : PagedInput
{
    public string? RoleName { get; set; }
    
    public string? RoleCode { get; set; }
    
    public bool? State { get; set; }
}