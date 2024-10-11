using Yi.AspNetCore.System;

namespace Yi.System.Services.Sys.Dtos;

public class TenantGetListInput : PagedInput
{
    public string? Name { get; set; }
}