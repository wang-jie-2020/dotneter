using Yi.AspNetCore.Core;

namespace Yi.Sys.Services.Monitor.Dtos;

public class LoginLogGetListInputVo : PagedInput
{
    public string? LoginUser { get; set; }

    public string? LoginIp { get; set; }
}