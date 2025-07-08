namespace Yi.System.Monitor.Dtos;

public class LoginLogQuery : PagedQuery
{
    public string? LoginUser { get; set; }

    public string? LoginIp { get; set; }
}