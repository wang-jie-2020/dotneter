using Yi.System.Services.Rbac.Dtos;

namespace Yi.System.Services.Rbac;

public class UserInfoCacheItem
{
    public UserInfoCacheItem(UserRoleMenuDto info)
    {
        Info = info;
    }

    /// <summary>
    ///     存储的用户信息
    /// </summary>
    public UserRoleMenuDto Info { get; set; }
}