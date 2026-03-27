namespace Yi.AspNetCore.MultiTenancy;

public interface ITenantStore
{
    Task<TenantConfiguration?> FindAsync(string normalizedName);

    Task<TenantConfiguration?> FindAsync(long id);
}
