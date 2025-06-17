using Microsoft.AspNetCore.Http;
using Yi.AspNetCore.Extensions;
using Yi.AspNetCore.Security;

namespace Yi.AspNetCore.Core.Permissions;

public class DefaultPermissionHandler : IPermissionHandler
{
    private readonly ICurrentUser _currentUser;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DefaultPermissionHandler(ICurrentUser currentUser, IHttpContextAccessor httpContextAccessor)
    {
        _currentUser = currentUser;
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsPass(string permission)
    {
        var permissions = _httpContextAccessor.HttpContext.GetUserPermissions(TokenClaimConst.Permission);
        if (permissions is not null)
        {
            if (permissions.Contains("*:*:*"))
            {
                return true;
            }

            return permissions.Contains(permission);
        }

        return false;
    }
}