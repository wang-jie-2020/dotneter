namespace Yi.AspNetCore.MultiTenancy;

public interface ITenantConfigurationProvider
{
    Task<TenantConfiguration?> GetAsync(bool saveResolveResult = false);
}
