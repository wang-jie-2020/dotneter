namespace Yi.Infra.Rbac.Dtos.Account;

public class UpdatePasswordDto
{
    public string NewPassword { get; set; } = string.Empty;
    public string OldPassword { get; set; } = string.Empty;
}