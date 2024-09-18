using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;
using Yi.AspNetCore.Extensions;

namespace Yi.AspNetCore.Permissions;

public class DefaultPermissionHandler : IPermissionHandler, ITransientDependency
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