using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.MultiTenancy.Resolver;

namespace Yi.AspNetCore.MultiTenancy;

public class TenantConfigurationProvider : ITenantConfigurationProvider, ITransientDependency
{
    public TenantConfigurationProvider(
        ITenantResolver tenantResolver,
        ITenantStore tenantStore)
    {
        TenantResolver = tenantResolver;
        TenantStore = tenantStore;
    }

    protected virtual ITenantResolver TenantResolver { get; }
    protected virtual ITenantStore TenantStore { get; }

    public virtual async Task<TenantConfiguration?> GetAsync(bool saveResolveResult = false)
    {
        //租户解析器获取到当前解析成功的租户
        var resolveResult = await TenantResolver.ResolveTenantIdOrNameAsync();

        TenantConfiguration? tenant = null;
        if (resolveResult.TenantIdOrName != null)
        {
            //根据租户信息获取租户
            tenant = await FindTenantAsync(resolveResult.TenantIdOrName);

            if (tenant == null || !tenant.IsActive)
            {
                //todo i18n
                throw new BusinessException(
                    "Volo.AbpIo.MultiTenancy:010001"
                    //StringLocalizer["TenantNotFoundMessage"],
                    //StringLocalizer["TenantNotFoundDetails", resolveResult.TenantIdOrName]
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