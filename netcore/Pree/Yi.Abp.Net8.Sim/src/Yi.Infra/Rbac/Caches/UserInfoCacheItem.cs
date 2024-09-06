using Yi.Abp.Infra.Rbac.Dtos;

namespace Yi.Abp.Infra.Rbac.Caches
{
    public class UserInfoCacheItem
    {
        public UserInfoCacheItem(UserRoleMenuDto info) { Info = info; }
        /// <summary>
        /// 存储的用户信息
        /// </summary>
        public UserRoleMenuDto Info { get; set; }
    }
    public class UserInfoCacheKey
    {
        public UserInfoCacheKey(Guid userId) { UserId = userId; }

        public Guid UserId { get; set; }

        public override string ToString()
        {
            return $"User:{UserId}";
        }
    }
}
