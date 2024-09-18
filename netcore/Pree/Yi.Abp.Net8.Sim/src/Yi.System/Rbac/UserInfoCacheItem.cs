using Yi.System.Rbac.Dtos;

namespace Yi.System.Rbac;

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