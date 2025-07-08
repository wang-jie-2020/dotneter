namespace Yi.System.Services.Dtos;

public class AccountInfo
{
    public UserDto User { get; set; } = new();
    
    public List<string> Roles { get; set; } = new();
    
    public List<string> Permissions { get; set; } = new();
}