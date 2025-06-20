using Yi.AspNetCore.Core;
using Yi.Framework.Core;

namespace Yi.System.Services.Dtos;

public class MenuGetListInput : PagedInput
{
    public bool? State { get; set; }
    
    public string? MenuName { get; set; }
}