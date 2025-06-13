namespace Volo.Abp.MultiTenancy;

public interface ITenantStore
{
    Task<TenantConfiguration?> FindAsync(string normalizedName);

    Task<TenantConfiguration?> FindAsync(Guid id);
}
