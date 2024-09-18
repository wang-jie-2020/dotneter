namespace Yi.System.TenantManagement.Dtos;

public class TenantGetListInput : PagedInfraInput
{
    public string? Name { get; set; }
}