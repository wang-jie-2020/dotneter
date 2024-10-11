namespace Yi.System.Services.Sys.Dtos;

public class LoginInputVo
{
    public string UserName { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;

    public string? Uuid { get; set; }

    public string? Code { get; set; }
}