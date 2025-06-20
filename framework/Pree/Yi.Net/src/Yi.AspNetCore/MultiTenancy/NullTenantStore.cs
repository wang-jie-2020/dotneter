
using Volo.Abp.DependencyInjection;

namespace Yi.AspNetCore.MultiTenancy;

[Dependency(TryRegister = true)]
public class NullTenantStore: ITenantStore, ITransientDependency
{
    public Task<TenantConfiguration?> FindAsync(string normalizedName)
    {
        return Task.FromResult<TenantConfiguration?>(null);
    }

    public Task<TenantConfiguration?> FindAsync(Guid id)
    {
        return Task.FromResult<TenantConfiguration?>(null);
    }
}