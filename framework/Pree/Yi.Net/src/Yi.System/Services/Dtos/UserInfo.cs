namespace Yi.System.Services.Dtos;

public class UserInfo
{
    public UserDto User { get; set; } = new();
    
    public List<string> Roles { get; set; } = new();
    
    public List<string> Permissions { get; set; } = new();
}