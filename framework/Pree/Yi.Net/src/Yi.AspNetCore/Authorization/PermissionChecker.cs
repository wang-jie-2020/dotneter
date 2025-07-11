using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.MultiTenancy;
using Yi.AspNetCore.Security;

namespace Yi.AspNetCore.Authorization;

public class PermissionChecker : IPermissionChecker, ITransientDependency
{
    protected IServiceProvider ServiceProvider { get; }
    protected ICurrentPrincipalAccessor PrincipalAccessor { get; }
    protected ICurrentTenant CurrentTenant { get; }
    protected PermissionOptions Options { get; }

    public PermissionChecker(
        IServiceProvider serviceProvider,
        ICurrentPrincipalAccessor principalAccessor,
        ICurrentTenant currentTenant,
        IOptions<PermissionOptions> options)
    {
        ServiceProvider = serviceProvider;
        PrincipalAccessor = principalAccessor;
        CurrentTenant = currentTenant;
        Options = options.Value;
    }

    public virtual async Task<bool> IsGrantedAsync(string name)
    {
        return await IsGrantedAsync(PrincipalAccessor.Principal, name);
    }

    public virtual async Task<bool> IsGrantedAsync(
        ClaimsPrincipal? claimsPrincipal,
        string name)
    {
        Check.NotNull(name, nameof(name));

        var isGranted = false;
        var context = new PermissionCheckContext(name, claimsPrincipal);
        foreach (var handlerType in Options.CheckHandlers)
        {
            var handler = (IPermissionCheckHandler)ServiceProvider.GetRequiredService(handlerType);
            if (await handler.CheckAsync(context))
            {
                return true;
            }
        }

        return isGranted;
    }
}