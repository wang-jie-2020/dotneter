using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Yi.AspNetCore;
using Yi.AspNetCore.Extensions.Caching;
using Yi.Framework.Abstractions;
using Yi.System.Entities;
using Yi.System.Options;
using Yi.System.Services.Dtos;
using Yi.System.Services.Impl;

namespace Yi.System.Domains;

public class UserManager : BaseDomain
{
    private readonly IDistributedCache _cache;
    private readonly ISqlSugarRepository<UserEntity> _userRepository;
    private readonly ISqlSugarRepository<UserPostEntity> _userPostRepository;
    private readonly ISqlSugarRepository<UserRoleEntity> _userRoleRepository;
    private readonly ISqlSugarRepository<RoleEntity> _roleRepository;

    public UserManager(
        IDistributedCache cache,
        ISqlSugarRepository<UserEntity> userRepository,
        ISqlSugarRepository<UserRoleEntity> userRoleRepository,
        ISqlSugarRepository<UserPostEntity> userPostRepository,
        ISqlSugarRepository<RoleEntity> roleRepository)
    {
        _cache = cache;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _userPostRepository = userPostRepository;
        _roleRepository = roleRepository;
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
        if (input.UserName == AccountConst.AdminName)
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
    
    public async Task<UserRoleMenuDto> GetInfoAsync(Guid userId)
    {
        var output = await GetInfoByCacheAsync(userId);
        return output;
    }

    private async Task<UserRoleMenuDto> GetInfoByCacheAsync(Guid userId)
    {
        var tokenExpiresMinuteTime = LazyServiceProvider.GetRequiredService<IOptions<JwtOptions>>().Value.ExpiresMinuteTime;
        var cacheData = await _cache.GetOrAddAsync(new UserInfoCacheKey(userId).ToString(),
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
                
                var data = EntityMapToDto(user);
                return new UserInfoCacheItem(data);
            },
            () => new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(tokenExpiresMinuteTime)
            });
        
        return cacheData.Info;
    }

    private UserRoleMenuDto EntityMapToDto(UserEntity user)
    {
        var userRoleMenu = new UserRoleMenuDto();

        user.EncryPassword.Password = string.Empty;
        user.EncryPassword.Salt = string.Empty;

        //超级管理员特殊处理
        if (user.Roles.Any(f => f.RoleCode.Equals(AccountConst.AdminRole)))
        {
            userRoleMenu.User = user.Adapt<UserDto>();
            userRoleMenu.RoleCodes.Add(AccountConst.AdminRole);
            userRoleMenu.PermissionCodes.Add("*:*:*");
            return userRoleMenu;
        }

        //得到角色集合
        var roleList = user.Roles;

        //得到菜单集合
        foreach (var role in roleList)
        {
            userRoleMenu.RoleCodes.Add(role.RoleCode);

            if (role.Menus is not null)
            {
                foreach (var menu in role.Menus)
                {
                    if (!string.IsNullOrEmpty(menu.PermissionCode))
                    {
                        userRoleMenu.PermissionCodes.Add(menu.PermissionCode);
                    }

                    userRoleMenu.Menus.Add(menu.Adapt<MenuDto>());
                }
            }

            //刚好可以去除一下多余的导航属性
            role.Menus = new List<MenuEntity>();
            userRoleMenu.Roles.Add(role.Adapt<RoleDto>());
        }

        user.Roles = new List<RoleEntity>();
        userRoleMenu.User = user.Adapt<UserDto>();
        userRoleMenu.Menus = userRoleMenu.Menus.OrderByDescending(x => x.OrderNum).ToHashSet();
        return userRoleMenu;
    }
}