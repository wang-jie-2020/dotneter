namespace Yi.System.Services.Dtos;

public class RoleAuthUserQuery : PagedQuery
{
    public string? UserName { get; set; }

    public long? Phone { get; set; }
}