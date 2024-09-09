namespace Yi.Infra.Rbac.Dtos.LoginLog;

public class LoginLogGetListInputVo : PagedInfraInput
{
    public string? LoginUser { get; set; }

    public string? LoginIp { get; set; }
}