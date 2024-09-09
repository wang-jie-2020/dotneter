using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;
using Yi.Framework.Core.Extensions;
using Yi.Infra.Rbac.Consts;

namespace Yi.Infra.Rbac.Authorization;

public class DefaultPermissionHandler : IPermissionHandler, ITransientDependency
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DefaultPermissionHandler(ICurrentUser currentUser, IHttpContextAccessor httpContextAccessor)
    {
        _currentUser = currentUser;
        _httpContextAccessor = httpContextAccessor;
    }

    private ICurrentUser _currentUser { get; set; }

    public bool IsPass(string permission)
    {
        var permissions = _httpContextAccessor.HttpContext.GetUserPermissions(TokenTypeConst.Permission);
        if (permissions is not null)
        {
            if (permissions.Contains("*:*:*")) return true;

            return permissions.Contains(permission);
        }

        return false;
    }
}