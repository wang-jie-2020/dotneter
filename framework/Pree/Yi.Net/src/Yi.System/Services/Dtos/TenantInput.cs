namespace Yi.System.Services.Dtos;

public class TenantInput
{
    public string Name { get; set; }
    
    public string? TenantConnectionString { get; set; }

    public DbType DbType { get; set; }
}