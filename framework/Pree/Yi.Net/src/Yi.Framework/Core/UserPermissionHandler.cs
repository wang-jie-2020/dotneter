using Volo.Abp.DependencyInjection;
using Yi.AspNetCore.Authorization;
using Yi.AspNetCore.Security;

namespace Yi.Framework.Core;

public class UserPermissionHandler : IPermissionCheckHandler, ITransientDependency
{
    private readonly IUserStore _userStore;

    public UserPermissionHandler(IUserStore userStore)
    {
        _userStore = userStore;
    }
    
    public async Task<bool> CheckAsync(PermissionCheckContext context)
    {
        var userId = context.Principal?.FindUserId();

        if (userId == null)
        {
            return false;
        }

        var userInfo = await _userStore.GetInfoAsync(userId.Value);

        var permissions = userInfo.Permissions;
        if (permissions is null) return false;
        if (permissions.Contains("*:*:*"))
        {
            return true;
        }

        return permissions.Contains(context.Permission);
    }
}