namespace Yi.System.Services.Dtos;

public class RoleAuthUserGetListQuery : PagedQuery
{
    public string? UserName { get; set; }

    public long? Phone { get; set; }
}