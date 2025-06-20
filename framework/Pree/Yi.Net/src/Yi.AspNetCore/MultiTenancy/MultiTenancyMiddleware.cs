using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Yi.AspNetCore.MultiTenancy;

public class MultiTenancyMiddleware : IMiddleware, ITransientDependency
{
    public ILogger<MultiTenancyMiddleware> Logger { get; set; }

    private readonly ITenantConfigurationProvider _tenantConfigurationProvider;
    private readonly ICurrentTenant _currentTenant;

    public MultiTenancyMiddleware(
        ITenantConfigurationProvider tenantConfigurationProvider,
        ICurrentTenant currentTenant)
    {
        Logger = NullLogger<MultiTenancyMiddleware>.Instance;

        _tenantConfigurationProvider = tenantConfigurationProvider;
        _currentTenant = currentTenant;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        TenantConfiguration? tenant = null;
        try
        {
            tenant = await _tenantConfigurationProvider.GetAsync();
        }
        catch (Exception e)
        {
            Logger.LogException(e);
            return;
        }

        if (tenant?.Id != _currentTenant.Id)
        {
            using (_currentTenant.Change(tenant?.Id, tenant?.Name))
            {
                await next(context);
            }
        }
        else
        {
            await next(context);
        }
    }
}
