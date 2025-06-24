using Yi.Framework;

namespace Yi.System.Services.Dtos;

public class TenantGetListInput : PagedInput
{
    public string? Name { get; set; }
}