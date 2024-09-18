using Yi.AspNetCore.Common;

namespace Yi.System.Services.TenantManagement.Dtos;

public class TenantGetListInput : PagedInfraInput
{
    public string? Name { get; set; }
}