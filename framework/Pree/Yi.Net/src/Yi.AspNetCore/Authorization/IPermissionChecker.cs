using System.Security.Claims;
using JetBrains.Annotations;

namespace Yi.AspNetCore.Authorization;

public interface IPermissionChecker
{
    Task<bool> IsGrantedAsync([NotNull] string name);
    
    Task<bool> IsGrantedAsync(ClaimsPrincipal? claimsPrincipal, [NotNull] string name);
}