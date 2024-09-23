using Yi.AspNetCore.System;

namespace Yi.System.Services.Monitor.Dtos;

public class LoginLogGetListInputVo : PagedInput
{
    public string? LoginUser { get; set; }

    public string? LoginIp { get; set; }
}