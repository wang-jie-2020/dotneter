namespace Yi.System.Services.Dtos;

public class LoginLogQuery : PagedQuery
{
    public string? LoginUser { get; set; }

    public string? LoginIp { get; set; }
}