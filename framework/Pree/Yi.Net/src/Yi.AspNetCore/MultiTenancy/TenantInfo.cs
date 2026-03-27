namespace Yi.AspNetCore.MultiTenancy;

public class TenantInfo
{
    public long? TenantId { get; }
    
    public string? Name { get; }

    public TenantInfo(long? tenantId, string? name = null)
    {
        TenantId = tenantId;
        Name = name;
    }
}
