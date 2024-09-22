using Yi.System.Services.System.Dtos;

namespace Yi.System.Services.System.Impl;

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