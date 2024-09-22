using Yi.AspNetCore.System;

namespace Yi.System.Services.System.Dtos;

public class TenantGetListInput : PagedInfraInput
{
    public string? Name { get; set; }
}