namespace Yi.System.Services.Dtos;

public class TenantGetListQuery : PagedQuery
{
    public string? Name { get; set; }
}