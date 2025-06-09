using Yi.AspNetCore.Core;

namespace Yi.Sys.Services.Infra.Dtos;

public class TenantGetListInput : PagedInput
{
    public string? Name { get; set; }
}