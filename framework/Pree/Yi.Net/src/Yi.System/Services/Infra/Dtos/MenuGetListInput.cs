using Yi.AspNetCore.Core;

namespace Yi.Sys.Services.Infra.Dtos;

public class MenuGetListInput : PagedInput
{
    public bool? State { get; set; }
    
    public string? MenuName { get; set; }
}