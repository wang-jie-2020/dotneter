using Yi.AspNetCore.System;

namespace Yi.System.Services.Rbac.Dtos;

public class LoginLogGetListInputVo : PagedInfraInput
{
    public string? LoginUser { get; set; }

    public string? LoginIp { get; set; }
}