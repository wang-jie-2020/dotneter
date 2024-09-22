namespace Yi.System.Services.System.Dtos;

public class UpdatePasswordDto
{
    public string NewPassword { get; set; } = string.Empty;
    
    public string OldPassword { get; set; } = string.Empty;
}