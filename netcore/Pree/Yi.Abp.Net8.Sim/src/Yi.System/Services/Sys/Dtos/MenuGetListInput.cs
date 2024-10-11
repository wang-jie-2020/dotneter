using Yi.AspNetCore.System;

namespace Yi.System.Services.Sys.Dtos;

public class MenuGetListInput : PagedInput
{
    public bool? State { get; set; }
    
    public string? MenuName { get; set; }
}