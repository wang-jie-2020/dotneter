using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Yi.AspNetCore.Data;

namespace Yi.AspNetCore.MultiTenancy;

public class MultiTenantConnectionStringResolver : DefaultConnectionStringResolver
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IServiceProvider _serviceProvider;

    public MultiTenantConnectionStringResolver(
        IOptionsMonitor<DbConnectionOptions> options,
        ICurrentTenant currentTenant,
        IServiceProvider serviceProvider)
        : base(options)
    {
        _currentTenant = currentTenant;
        _serviceProvider = serviceProvider;
    }

    public override async Task<string> ResolveAsync(string? connectionStringName = null)
    {
        if (_currentTenant.Id == null)
        {
            return await base.ResolveAsync(connectionStringName);
        }

        var tenant = await FindTenantConfigurationAsync(_currentTenant.Id.Value);
        if (tenant == null || tenant.ConnectionStrings.IsNullOrEmpty())
        {
            return await base.ResolveAsync(connectionStringName);
        }

        var tenantDefaultConnectionString = tenant.ConnectionStrings?.Default;
        if (connectionStringName == null || connectionStringName == ConnectionStrings.DefaultConnectionStringName)
        {
            return !tenantDefaultConnectionString.IsNullOrWhiteSpace()
                ? tenantDefaultConnectionString!
                : Options.ConnectionStrings.Default!;
        }
        
        var connString = tenant.ConnectionStrings?.FirstOrDefault().Value;
        if (!connString.IsNullOrWhiteSpace())
        {
            return connString!;
        }
        
        if (!tenantDefaultConnectionString.IsNullOrWhiteSpace())
        {
            return tenantDefaultConnectionString!;
        }

        return await base.ResolveAsync(connectionStringName);
    }

    protected virtual async Task<TenantConfiguration?> FindTenantConfigurationAsync(Guid tenantId)
    {
        using (var serviceScope = _serviceProvider.CreateScope())
        {
            var tenantStore = serviceScope.ServiceProvider.GetRequiredService<ITenantStore>();
            return await tenantStore.FindAsync(tenantId);
        }
    }
}