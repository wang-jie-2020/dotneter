namespace Yi.System.Services.Dtos;

public class ProfilePasswordInput
{
    public string NewPassword { get; set; } = string.Empty;
    
    public string OldPassword { get; set; } = string.Empty;
}