using Yi.AspNetCore.System;

namespace Yi.System.Services.System.Dtos;

public class TenantGetListInput : PagedInput
{
    public string? Name { get; set; }
}