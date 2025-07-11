using Yi.AspNetCore.Authorization;
using Yi.AspNetCore.Security;

namespace Yi.System.Domains;

public class UserPermissionHandler : IPermissionCheckHandler, ITransientDependency
{
    private readonly UserManager _userManager;

    public UserPermissionHandler(UserManager userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<bool> CheckAsync(PermissionCheckContext context)
    {
        var userId = context.Principal?.FindUserId();

        if (userId == null)
        {
            return false;
        }

        var userInfo = await _userManager.GetInfoAsync(userId.Value);

        var permissions = userInfo.Permissions;
        if (permissions is null) return false;
        if (permissions.Contains("*:*:*"))
        {
            return true;
        }

        return permissions.Contains(context.Permission);
    }
}