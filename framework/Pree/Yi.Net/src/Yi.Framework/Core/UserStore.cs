using Microsoft.Extensions.Caching.Distributed;
using Yi.AspNetCore;
using Yi.AspNetCore.Extensions.Caching;
using Yi.Framework.Core.Entities;
using Yi.Framework.SqlSugarCore.Repositories;

namespace Yi.Framework.Core;

public class UserStore : IUserStore
{
    private readonly IDistributedCache _cache;
    private readonly ISqlSugarRepository<UserEntity> _userRepository;
    private readonly ISqlSugarRepository<MenuEntity> _menuRepository;
    
    public UserStore(
        IDistributedCache cache,
        ISqlSugarRepository<UserEntity> userRepository,
        ISqlSugarRepository<MenuEntity> menuRepository)
    {
        _cache = cache;
        _userRepository = userRepository;
        _menuRepository = menuRepository;
    }
    
    public async Task<UserConfiguration> GetInfoAsync(Guid userId, bool refreshCache = false)
    {
        if (refreshCache)
        {
            var cacheKay = UserCacheItem.CalculateCacheKey(userId);
            await _cache.RemoveAsync(cacheKay);
        }
        
        var output = await GetInfoByCacheAsync(userId);
        return output;
    }
    
    private async Task<UserConfiguration> GetInfoByCacheAsync(Guid userId)
    {
        var cacheData = await _cache.GetOrAddAsync(UserCacheItem.CalculateCacheKey(userId),
            async () =>
            {
                var user = await _userRepository.AsQueryable()
                    .Includes(
                        u => u.Roles.Where(r => r.IsDeleted == false).ToList(),
                        r => r.Menus.Where(m => m.IsDeleted == false).ToList()
                    )
                    .InSingleAsync(userId);

                if (user is null)
                {
                    throw new UnauthorizedException();
                }

                var data = UserEntityMapping(user);
                return new UserCacheItem(data);
            },
            () => new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
            });

        return cacheData.Info;
    }
    
    private UserConfiguration UserEntityMapping(UserEntity user)
    {
        var authorities = new UserConfiguration();

        authorities.User = user;
        foreach (var role in user.Roles)
        {
            authorities.Roles.Add(role);
            foreach (var menu in role.Menus)
            {
                if (!authorities.Menus.Any(t => t.Id == menu.Id))
                {
                    authorities.Menus.Add(menu);

                    if (!menu.PermissionCode.IsNullOrEmpty())
                    {
                        authorities.Permissions.Add(menu.PermissionCode);
                    }
                }
            }
        }

        authorities.Menus = authorities.Menus.OrderBy(x => x.OrderNum).ToList();

        //管理员特殊处理
        if (authorities.IsAdmin())
        {
            authorities.Menus = _menuRepository.GetList();
            authorities.Permissions = ["*:*:*"];
        }

        return authorities;
    }
    
    public class UserCacheItem
    {
        public UserCacheItem(UserConfiguration info)
        {
            Info = info;
        }

        /// <summary>
        ///     存储的用户信息
        /// </summary>
        public UserConfiguration Info { get; set; }

        public static string CalculateCacheKey(Guid? id)
        {
            if (id == null) throw new Exception("id can't be invalid.");

            return $"User:{id}";
        }
    }
}