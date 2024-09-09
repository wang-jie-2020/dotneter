namespace Yi.Infra.TenantManagement.Dtos;

public class TenantGetListInput : PagedInfraInput
{
    public string? Name { get; set; }
}