using Yi.AspNetCore.Common;

namespace Yi.System.Services.Account.Dtos;

public class LoginLogGetListInputVo : PagedInfraInput
{
    public string? LoginUser { get; set; }

    public string? LoginIp { get; set; }
}