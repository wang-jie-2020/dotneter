using Yi.Framework.Ddd.Application;

namespace Yi.Infra.Rbac.Dtos.LoginLog;

public class LoginLogGetListInputVo : PagedRequestInput
{
    public string? LoginUser { get; set; }

    public string? LoginIp { get; set; }
}