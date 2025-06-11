using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

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

    [Obsolete("Use ResolveAsync method.")]
    public override string Resolve(string? connectionStringName = null)
    {
        //No current tenant, fallback to default logic
        if (_currentTenant.Id == null)
        {
            return base.Resolve(connectionStringName);
        }

        var tenant = FindTenantConfiguration(_currentTenant.Id.Value);

        //Tenant has not defined any connection string, fallback to default logic
        if (tenant == null || tenant.ConnectionStrings.IsNullOrEmpty())
        {
            return base.Resolve(connectionStringName);
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
        var connString = tenant.ConnectionStrings?.GetOrDefault(connectionStringName);
        if (!connString.IsNullOrWhiteSpace())
        {
            return connString!;
        }

        //Fallback to tenant's default connection string if available
        if (!tenantDefaultConnectionString.IsNullOrWhiteSpace())
        {
            return tenantDefaultConnectionString!;
        }

        //Try to find the specific connection string for given name
        var connStringInOptions = Options.ConnectionStrings.GetOrDefault(connectionStringName);
        if (!connStringInOptions.IsNullOrWhiteSpace())
        {
            return connStringInOptions!;
        }

        //Fallback to the global default connection string
        var defaultConnectionString = Options.ConnectionStrings.Default;
        if (!defaultConnectionString.IsNullOrWhiteSpace())
        {
            return defaultConnectionString!;
        }

        throw new AbpException("No connection string defined!");
    }

    protected virtual async Task<TenantConfiguration?> FindTenantConfigurationAsync(Guid tenantId)
    {
        using (var serviceScope = _serviceProvider.CreateScope())
        {
            var tenantStore = serviceScope.ServiceProvider.GetRequiredService<ITenantStore>();
            return await tenantStore.FindAsync(tenantId);
        }
    }

    [Obsolete("Use FindTenantConfigurationAsync method.")]
    protected virtual TenantConfiguration? FindTenantConfiguration(Guid tenantId)
    {
        using (var serviceScope = _serviceProvider.CreateScope())
        {
            var tenantStore = serviceScope.ServiceProvider.GetRequiredService<ITenantStore>();
            return tenantStore.Find(tenantId);
        }
    }
}