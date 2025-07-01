using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.MultiTenancy.Resolver;

namespace Yi.AspNetCore.MultiTenancy;

public class TenantConfigurationProvider : ITenantConfigurationProvider, ITransientDependency
{
    protected virtual ITenantResolver TenantResolver { get; }
    protected virtual ITenantStore TenantStore { get; }

    public TenantConfigurationProvider(
        ITenantResolver tenantResolver,
        ITenantStore tenantStore)
    {
        TenantResolver = tenantResolver;
        TenantStore = tenantStore;
    }

    public virtual async Task<TenantConfiguration?> GetAsync()
    {
        var resolveResult = await TenantResolver.ResolveTenantIdOrNameAsync();

        TenantConfiguration? tenant = null;
        if (resolveResult.TenantIdOrName != null)
        {
            tenant = await FindTenantAsync(resolveResult.TenantIdOrName);

            if (tenant == null || !tenant.IsActive)
            {
                throw Oops.Oh("TenantNotFound").WithData("tenant", resolveResult.TenantIdOrName);
            }
        }

        return tenant;
    }

    protected virtual async Task<TenantConfiguration?> FindTenantAsync(string tenantIdOrName)
    {
        if (Guid.TryParse(tenantIdOrName, out var parsedTenantId))
        {
            return await TenantStore.FindAsync(parsedTenantId);
        }

        return await TenantStore.FindAsync(tenantIdOrName);
    }
}