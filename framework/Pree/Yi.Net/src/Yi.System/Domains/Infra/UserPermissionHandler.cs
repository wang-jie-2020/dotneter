using Volo.Abp.Users;
using Yi.AspNetCore.System.Permissions;

namespace Yi.Sys.Domains.Infra;

public class UserPermissionHandler : IPermissionHandler
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

        var userInfo = _userManager.GetInfoAsync(_currentUser.Id.Value).Result;
        
        var permissions = userInfo.PermissionCodes;
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