using System.Security.Claims;

namespace Yi.AspNetCore.Security;

public interface ICurrentPrincipalAccessor
{
    ClaimsPrincipal Principal { get; }

    IDisposable Change(ClaimsPrincipal principal);
}
