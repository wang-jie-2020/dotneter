namespace Yi.Sys.Services.Infra.Dtos;

public class UpdatePasswordDto
{
    public string NewPassword { get; set; } = string.Empty;
    
    public string OldPassword { get; set; } = string.Empty;
}