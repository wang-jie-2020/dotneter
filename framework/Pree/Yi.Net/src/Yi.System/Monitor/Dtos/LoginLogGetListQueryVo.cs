namespace Yi.System.Monitor.Dtos;

public class LoginLogGetListQueryVo : PagedQuery
{
    public string? LoginUser { get; set; }

    public string? LoginIp { get; set; }
}