using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;

namespace Yi.AspNetCore.Security;

public class HttpContextCurrentPrincipalAccessor : CurrentPrincipalAccessorBase, ISingletonDependency
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextCurrentPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override ClaimsPrincipal GetClaimsPrincipal()
    {
        return _httpContextAccessor.HttpContext?.User ?? (Thread.CurrentPrincipal as ClaimsPrincipal)!;
    }
}
