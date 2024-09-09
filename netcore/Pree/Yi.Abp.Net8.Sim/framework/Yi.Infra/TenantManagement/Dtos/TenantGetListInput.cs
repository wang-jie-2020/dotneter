namespace Yi.Infra.TenantManagement.Dtos;

public class TenantGetListInput : PagedInfraInput
{
    public string? Name { get; set; }
    //public int? EntityVersion { get;  set; }

    //public string? TenantConnectionString { get;  set; }

    //public DbType? DbType { get;  set; }
}