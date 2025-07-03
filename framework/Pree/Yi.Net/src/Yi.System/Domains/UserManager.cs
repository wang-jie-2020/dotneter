using System.Text.RegularExpressions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Yi.AspNetCore;
using Yi.AspNetCore.Extensions.Caching;
using Yi.Framework.Abstractions;
using Yi.System.Domains.Entities;
using Yi.System.Domains.Repositories;
using Yi.System.Options;
using Yi.System.Services.Dtos;
using Yi.System.Services.Impl;

namespace Yi.System.Domains;

public class UserManager : BaseDomain
{
    private readonly ISqlSugarRepository<UserEntity> _repository;
    private readonly ISqlSugarRepository<UserPostEntity> _repositoryUserPost;
    private readonly ISqlSugarRepository<UserRoleEntity> _repositoryUserRole;
    private readonly ISqlSugarRepository<RoleEntity> _roleRepository;
    private readonly IDistributedCache _cache;
    private readonly IUserRepository _userRepository;

    public UserManager(ISqlSugarRepository<UserEntity> repository,
        ISqlSugarRepository<UserRoleEntity> repositoryUserRole, ISqlSugarRepository<UserPostEntity> repositoryUserPost,
        IDistributedCache cache,
        IUserRepository userRepository,
        ISqlSugarRepository<RoleEntity> roleRepository)
    {
        (_repository, _repositoryUserRole, _repositoryUserPost, _cache, _userRepository,
                 _roleRepository) =
            (repository, repositoryUserRole, repositoryUserPost, cache, userRepository,
                 roleRepository);
    }

    /// <summary>
    ///     给用户设置角色
    /// </summary>
    /// <param name="userIds"></param>
    /// <param name="roleIds"></param>
    /// <returns></returns>
    public async Task GiveUserSetRoleAsync(List<Guid> userIds, List<Guid> roleIds)
    {
        //删除用户之前所有的用户角色关系（物理删除，没有恢复的必要）
        await _repositoryUserRole.DeleteAsync(u => userIds.Contains(u.UserId));

        if (roleIds is not null)
            //遍历用户
            foreach (var userId in userIds)
            {
                //添加新的关系
                List<UserRoleEntity> userRoleEntities = new();

                foreach (var roleId in roleIds)
                    userRoleEntities.Add(new UserRoleEntity { UserId = userId, RoleId = roleId });
                //一次性批量添加
                await _repositoryUserRole.InsertRangeAsync(userRoleEntities);
            }
    }

    /// <summary>
    ///     给用户设置岗位
    /// </summary>
    /// <param name="userIds"></param>
    /// <param name="postIds"></param>
    /// <returns></returns>
    public async Task GiveUserSetPostAsync(List<Guid> userIds, List<Guid> postIds)
    {
        //删除用户之前所有的用户角色关系（物理删除，没有恢复的必要）
        await _repositoryUserPost.DeleteAsync(u => userIds.Contains(u.UserId));
        if (postIds is not null)
            //遍历用户
            foreach (var userId in userIds)
            {
                //添加新的关系
                List<UserPostEntity> userPostEntities = new();
                foreach (var post in postIds)
                    userPostEntities.Add(new UserPostEntity { UserId = userId, PostId = post });

                //一次性批量添加
                await _repositoryUserPost.InsertRangeAsync(userPostEntities);
            }
    }

    /// <summary>
    ///     创建用户
    /// </summary>
    /// <returns></returns>
    public async Task CreateAsync(UserEntity userEntity)
    {
        //校验用户名
        ValidateUserName(userEntity);

        if (userEntity.EncryPassword?.Password.Length < 6)
        {
            throw Oops.Oh(SystemErrorCodes.UserPasswordTooShort);
        }

        if (userEntity.Phone is not null)
        {
            if (await _repository.IsAnyAsync(x => x.Phone == userEntity.Phone))
            {
                throw Oops.Oh(SystemErrorCodes.UserPhoneRepeated);
            }
        }

        var isExist = await _repository.IsAnyAsync(x => x.UserName == userEntity.UserName);
        if (isExist)
        {
            throw Oops.Oh(SystemErrorCodes.UserNameRepeated);
        }

        await _repository.InsertReturnEntityAsync(userEntity);
    }

    public async Task SetDefaultRoleAsync(Guid userId)
    {
        var role = await _roleRepository.GetFirstAsync(x => x.RoleCode == AccountConst.DefaultRoleCode);
        if (role is not null)
        {
            await GiveUserSetRoleAsync(new List<Guid> { userId }, new List<Guid> { role.Id });
        }
    }

    private void ValidateUserName(UserEntity input)
    {
        if (input.UserName == AccountConst.Admin)
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

    /// <summary>
    ///     查询用户信息，已缓存
    /// </summary>
    /// <returns></returns>
    public async Task<UserRoleMenuDto> GetInfoAsync(Guid userId)
    {
        var output = await GetInfoByCacheAsync(userId);
        return output;
    }

    private async Task<UserRoleMenuDto> GetInfoByCacheAsync(Guid userId)
    {
        //此处优先从缓存中获取
        UserRoleMenuDto output = null;
        var tokenExpiresMinuteTime = LazyServiceProvider.GetRequiredService<IOptions<JwtOptions>>().Value.ExpiresMinuteTime;
        var cacheData = await _cache.GetOrAddAsync(new UserInfoCacheKey(userId).ToString(),
            async () =>
            {
                var user = await _userRepository.GetUserAllInfoAsync(userId);
                var data = EntityMapToDto(user);
                //系统用户数据被重置，老前端访问重新授权
                if (data is null)
                {
                    throw new UnauthorizedException();
                }
                //data.Menus.Clear();
                output = data;
                return new UserInfoCacheItem(data);
            },
            () => new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(tokenExpiresMinuteTime)
            });

        if (cacheData is not null)
        {
            output = cacheData.Info;
        }

        return output!;
    }

    /// <summary>
    ///     批量查询用户信息
    /// </summary>
    /// <param name="userIds"></param>
    /// <returns></returns>
    public async Task<List<UserRoleMenuDto>> GetInfoListAsync(List<Guid> userIds)
    {
        var output = new List<UserRoleMenuDto>();
        foreach (var userId in userIds)
        {
            output.Add(await GetInfoByCacheAsync(userId));
        }

        return output;
    }

    private UserRoleMenuDto EntityMapToDto(UserEntity user)
    {
        var userRoleMenu = new UserRoleMenuDto();

        user.EncryPassword.Password = string.Empty;
        user.EncryPassword.Salt = string.Empty;

        //超级管理员特殊处理
        if (AccountConst.Admin.Equals(user.UserName))
        {
            userRoleMenu.User = user.Adapt<UserDto>();
            userRoleMenu.RoleCodes.Add(AccountConst.AdminRoleCode);
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