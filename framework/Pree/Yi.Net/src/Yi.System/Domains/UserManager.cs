using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Yi.AspNetCore;
using Yi.AspNetCore.Extensions.Caching;
using Yi.Framework.Abstractions;
using Yi.System.Entities;
using Yi.System.Options;

namespace Yi.System.Domains;

public class UserManager : BaseDomain
{
    private readonly IDistributedCache _cache;
    private readonly ISqlSugarRepository<UserEntity> _userRepository;
    private readonly ISqlSugarRepository<UserPostEntity> _userPostRepository;
    private readonly ISqlSugarRepository<UserRoleEntity> _userRoleRepository;
    private readonly ISqlSugarRepository<RoleEntity> _roleRepository;
    private readonly ISqlSugarRepository<MenuEntity> _menuRepository;

    public UserManager(
        IDistributedCache cache,
        ISqlSugarRepository<UserEntity> userRepository,
        ISqlSugarRepository<UserRoleEntity> userRoleRepository,
        ISqlSugarRepository<UserPostEntity> userPostRepository,
        ISqlSugarRepository<RoleEntity> roleRepository,
        ISqlSugarRepository<MenuEntity> menuRepository)
    {
        _cache = cache;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _userPostRepository = userPostRepository;
        _roleRepository = roleRepository;
        _menuRepository = menuRepository;
    }

    public async Task GiveUserSetRoleAsync(List<Guid> userIds, List<Guid> roleIds)
    {
        await _userRoleRepository.DeleteAsync(u => userIds.Contains(u.UserId));

        if (roleIds is not null)
        {
            foreach (var userId in userIds)
            {
                List<UserRoleEntity> userRoleEntities = new();
                foreach (var roleId in roleIds)
                {
                    userRoleEntities.Add(new UserRoleEntity { UserId = userId, RoleId = roleId });
                }

                await _userRoleRepository.InsertRangeAsync(userRoleEntities);
            }
        }
    }

    public async Task GiveUserSetPostAsync(List<Guid> userIds, List<Guid> postIds)
    {
        await _userPostRepository.DeleteAsync(u => userIds.Contains(u.UserId));
        if (postIds is not null)
        {
            foreach (var userId in userIds)
            {
                List<UserPostEntity> userPostEntities = new();
                foreach (var post in postIds)
                {
                    userPostEntities.Add(new UserPostEntity { UserId = userId, PostId = post });
                }

                await _userPostRepository.InsertRangeAsync(userPostEntities);
            }
        }
    }

    public async Task CreateAsync(UserEntity userEntity)
    {
        ValidateUserName(userEntity);

        if (userEntity.EncryPassword?.Password.Length < 6)
        {
            throw Oops.Oh(SystemErrorCodes.UserPasswordTooShort);
        }

        if (userEntity.Phone is not null)
        {
            if (await _userRepository.IsAnyAsync(x => x.Phone == userEntity.Phone))
            {
                throw Oops.Oh(SystemErrorCodes.UserPhoneRepeated);
            }
        }

        var isExist = await _userRepository.IsAnyAsync(x => x.UserName == userEntity.UserName);
        if (isExist)
        {
            throw Oops.Oh(SystemErrorCodes.UserNameRepeated);
        }

        await _userRepository.InsertReturnEntityAsync(userEntity);
    }

    public async Task SetDefaultRoleAsync(Guid userId)
    {
        var role = await _roleRepository.GetFirstAsync(x => x.RoleCode == AccountConst.DefaultRole);
        if (role is not null)
        {
            await GiveUserSetRoleAsync(new List<Guid> { userId }, new List<Guid> { role.Id });
        }
    }

    private void ValidateUserName(UserEntity input)
    {
        if (AccountConst.ForbiddenNames.Contains(input.UserName))
        {
            throw Oops.Oh(SystemErrorCodes.UserNameForbidden);
        }

        if (input.UserName.Length < 2)
        {
            throw Oops.Oh(SystemErrorCodes.UserNameTooShort);
        }

        // 正则表达式，匹配只包含数字和字母的字符串
        var pattern = @"^[a-zA-Z0-9]+$";

        var isMatch = Regex.IsMatch(input.UserName, pattern);
        if (!isMatch)
        {
            throw Oops.Oh(SystemErrorCodes.UserNameInvalid);
        }
    }

    public async Task RemoveCacheAsync(Guid userId)
    {
        var cacheKay = UserInfoCacheItem.CalculateCacheKey(userId);
        await _cache.RemoveAsync(cacheKay);
    }

    public async Task<UserAuthorities> GetInfoAsync(Guid userId)
    {
        var output = await GetInfoByCacheAsync(userId);
        return output;
    }

    private async Task<UserAuthorities> GetInfoByCacheAsync(Guid userId)
    {
        var tokenExpiresMinuteTime = LazyServiceProvider.GetRequiredService<IOptions<JwtOptions>>().Value.ExpiresMinuteTime;
        var cacheData = await _cache.GetOrAddAsync(UserInfoCacheItem.CalculateCacheKey(userId),
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
                return new UserInfoCacheItem(data);
            },
            () => new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(tokenExpiresMinuteTime)
            });

        return cacheData.Info;
    }

    private UserAuthorities UserEntityMapping(UserEntity user)
    {
        var authorities = new UserAuthorities();

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

    public class UserInfoCacheItem
    {
        public UserInfoCacheItem(UserAuthorities info)
        {
            Info = info;
        }

        /// <summary>
        ///     存储的用户信息
        /// </summary>
        public UserAuthorities Info { get; set; }

        public static string CalculateCacheKey(Guid? id)
        {
            if (id == null) throw new Exception("id can't be invalid.");

            return $"User:{id}";
        }
    }
}