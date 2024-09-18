using Yi.AspNetCore.Common;

namespace Yi.Admin.Services.TenantManagement.Dtos;

public class TenantGetListInput : PagedInfraInput
{
    public string? Name { get; set; }
}