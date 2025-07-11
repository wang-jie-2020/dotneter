using Yi.AspNetCore.Authorization;
using Yi.AspNetCore.Security;
using Yi.Framework.Permissions;

namespace Yi.System.Domains;

public class UserPermissionHandler : IPermissionHandler, IPermissionCheckHandler, ITransientDependency
{
    private readonly ICurrentUser _currentUser;
    private readonly UserManager _userManager;

    public UserPermissionHandler(ICurrentUser currentUser, UserManager userManager)
    {
        _currentUser = currentUser;
        _userManager = userManager;
    }

    public bool IsPass(string permission)
    {
        if (_currentUser.Id == null)
        {
            return false;
        }

        // 也许缓存形式更好
        var userInfo = _userManager.GetInfoAsync(_currentUser.Id.Value).Result;

        var permissions = userInfo.Permissions;
        if (permissions is null) return false;
        if (permissions.Contains("*:*:*"))
        {
            return true;
        }

        return permissions.Contains(permission);
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