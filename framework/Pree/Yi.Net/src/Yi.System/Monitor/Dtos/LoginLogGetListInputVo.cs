namespace Yi.System.Monitor.Dtos;

public class LoginLogGetListInputVo : PagedInput
{
    public string? LoginUser { get; set; }

    public string? LoginIp { get; set; }
}