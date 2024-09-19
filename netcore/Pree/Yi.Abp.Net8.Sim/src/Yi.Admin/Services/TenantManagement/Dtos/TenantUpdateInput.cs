namespace Yi.Admin.Services.TenantManagement.Dtos;

public class TenantUpdateInput
{
    public string? Name { get; set; }

    public string? TenantConnectionString { get; set; }

    public DbType? DbType { get; set; }
}