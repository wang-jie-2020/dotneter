using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.MultiTenancy.Resolver;

namespace Yi.AspNetCore.MultiTenancy;

public class TenantConfigurationProvider : ITenantConfigurationProvider, ITransientDependency
{
    protected virtual ITenantResolver TenantResolver { get; }
    protected virtual ITenantStore TenantStore { get; }
    
    protected virtual IStringLocalizer StringLocalizer { get; }

    public TenantConfigurationProvider(
        ITenantResolver tenantResolver,
        ITenantStore tenantStore,
        IStringLocalizer stringLocalizer)
    {
        TenantResolver = tenantResolver;
        TenantStore = tenantStore;
        StringLocalizer = stringLocalizer;
    }

    public virtual async Task<TenantConfiguration?> GetAsync()
    {
        var resolveResult = await TenantResolver.ResolveTenantIdOrNameAsync();

        TenantConfiguration? tenant = null;
        if (resolveResult.TenantIdOrName != null)
        {
            tenant = await FindTenantAsync(resolveResult.TenantIdOrName);

            if (tenant == null)
            {
                throw new BusinessException(
                    code: "MultiTenancy:010001",
                    message: StringLocalizer["TenantNotFoundMessage"],
                    details: StringLocalizer["TenantNotFoundDetails", resolveResult.TenantIdOrName]
                );
            }
            
            if (!tenant.IsActive)
            {
                throw new BusinessException(
                    code: "MultiTenancy:010002",
                    message: StringLocalizer["TenantNotActiveMessage"],
                    details: StringLocalizer["TenantNotActiveDetails", resolveResult.TenantIdOrName]
                );
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