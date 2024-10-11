using Yi.AspNetCore.System;

namespace Yi.System.Services.Sys.Dtos;

public class RoleGetListInput : PagedInput
{
    public string? RoleName { get; set; }
    
    public string? RoleCode { get; set; }
    
    public bool? State { get; set; }
}