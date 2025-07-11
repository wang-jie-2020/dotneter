using System.Security.Claims;
using JetBrains.Annotations;

namespace Yi.AspNetCore.Authorization;

public class PermissionCheckContext
{
    [NotNull]
    public string Permission { get; }

    public ClaimsPrincipal? Principal { get; }

    public PermissionCheckContext(
        [NotNull] string permission,
        ClaimsPrincipal? principal)
    {
        Check.NotNull(permission, nameof(permission));

        Permission = permission;
        Principal = principal;
    }
}