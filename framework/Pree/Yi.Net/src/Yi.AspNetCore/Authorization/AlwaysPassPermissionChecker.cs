using System.Security.Claims;
using Volo.Abp.DependencyInjection;

namespace Yi.AspNetCore.Authorization;

[Dependency(TryRegister = true)]
public class AlwaysPassPermissionChecker : IPermissionChecker, ITransientDependency
{
    public async Task<bool> IsGrantedAsync(string name)
    {
        return true;
    }

    public async Task<bool> IsGrantedAsync(ClaimsPrincipal? claimsPrincipal, string name)
    {
        return true;
    }
}