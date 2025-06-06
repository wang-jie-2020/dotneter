namespace Yi.Sys.Services.Infra.Dtos;

public class TenantUpdateInput
{
    public string? Name { get; set; }

    public string? TenantConnectionString { get; set; }

    public DbType? DbType { get; set; }
}