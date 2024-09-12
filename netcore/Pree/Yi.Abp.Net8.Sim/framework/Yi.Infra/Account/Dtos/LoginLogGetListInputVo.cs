namespace Yi.Infra.Account.Dtos;

public class LoginLogGetListInputVo : PagedInfraInput
{
    public string? LoginUser { get; set; }

    public string? LoginIp { get; set; }
}