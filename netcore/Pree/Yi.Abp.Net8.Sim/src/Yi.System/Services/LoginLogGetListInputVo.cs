using Yi.AspNetCore.System;

namespace Yi.System.Services;

public class LoginLogGetListInputVo : PagedInfraInput
{
    public string? LoginUser { get; set; }

    public string? LoginIp { get; set; }
}