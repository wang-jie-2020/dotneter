using Yi.AspNetCore.Core;
using Yi.Framework.Core;

namespace Yi.System.Services.Dtos;

public class TenantGetListInput : PagedInput
{
    public string? Name { get; set; }
}