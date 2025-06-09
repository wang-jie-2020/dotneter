namespace Yi.System.Services.Dtos;

public class TenantDto 
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public int EntityVersion { get; set; }

    public string TenantConnectionString { get; set; }

    public DbType DbType { get; set; }

    public DateTime CreationTime { get; set; }
}