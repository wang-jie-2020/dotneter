using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Yi.AspNetCore.MultiTenancy;

namespace Yi.System.Domains;

public class MultiTenantConnectionStringResolver : DefaultConnectionStringResolver
{
    private readonly ICurrentTenant _currentTenant;
    private readonly IServiceProvider _serviceProvider;

    public MultiTenantConnectionStringResolver(
        IOptionsMonitor<AbpDbConnectionOptions> options,
        ICurrentTenant currentTenant,
        IServiceProvider serviceProvider)
        : base(options)
    {
        _currentTenant = currentTenant;
        _serviceProvider = serviceProvider;
    }

    public override async Task<string> ResolveAsync(string? connectionStringName = null)
    {
        //No current tenant, fallback to default logic
        if (_currentTenant.Id == null)
        {
            return await base.ResolveAsync(connectionStringName);
        }

        var tenant = await FindTenantConfigurationAsync(_currentTenant.Id.Value);

        //Tenant has not defined any connection string, fallback to default logic
        if (tenant == null || tenant.ConnectionStrings.IsNullOrEmpty())
        {
            return await base.ResolveAsync(connectionStringName);
        }

        var tenantDefaultConnectionString = tenant.ConnectionStrings?.Default;

        //Requesting default connection string...
        if (connectionStringName == null || connectionStringName == ConnectionStrings.DefaultConnectionStringName)
        {
            return !tenantDefaultConnectionString.IsNullOrWhiteSpace()
                ? tenantDefaultConnectionString!
                : Options.ConnectionStrings.Default!;
        }

        //Requesting specific connection string...
        var connString = tenant.ConnectionStrings?.FirstOrDefault().Value;
        if (!connString.IsNullOrWhiteSpace())
        {
            return connString!;
        }

        //Fallback to the mapped database for the specific connection string
        var database = Options.Databases.GetMappedDatabaseOrNull(connectionStringName);
        if (database != null && database.IsUsedByTenants)
        {
            connString = tenant.ConnectionStrings?.GetOrDefault(database.DatabaseName);
            if (!connString.IsNullOrWhiteSpace())
            {
                return connString!;
            }
        }

        //Fallback to tenant's default connection string if available
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