using Volo.Abp.DependencyInjection;

namespace Yi.AspNetCore.MultiTenancy;

public class CurrentTenant : ICurrentTenant, ITransientDependency
{
    public virtual long? Id => _currentTenantAccessor.Current?.TenantId;

    public string? Name => _currentTenantAccessor.Current?.Name;

    private readonly ICurrentTenantAccessor _currentTenantAccessor;

    public CurrentTenant(ICurrentTenantAccessor currentTenantAccessor)
    {
        _currentTenantAccessor = currentTenantAccessor;
    }

    public IDisposable Change(long? id, string? name = null)
    {
        return SetCurrent(id, name);
    }

    private IDisposable SetCurrent(long? tenantId, string? name = null)
    {
        var parentScope = _currentTenantAccessor.Current;
        _currentTenantAccessor.Current = new TenantInfo(tenantId, name);

        return new DisposeAction<ValueTuple<ICurrentTenantAccessor, TenantInfo?>>(static (state) =>
        {
            var (currentTenantAccessor, parentScope) = state;
            currentTenantAccessor.Current = parentScope;
        }, (_currentTenantAccessor, parentScope));
    }
}
