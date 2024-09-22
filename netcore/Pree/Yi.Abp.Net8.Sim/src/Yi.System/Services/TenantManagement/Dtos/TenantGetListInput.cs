using Yi.AspNetCore.System;

namespace Yi.System.Services.TenantManagement.Dtos;

public class TenantGetListInput : PagedInfraInput
{
    public string? Name { get; set; }
}