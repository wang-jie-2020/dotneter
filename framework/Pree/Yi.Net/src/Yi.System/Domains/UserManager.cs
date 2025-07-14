using System.Text.RegularExpressions;
using Yi.AspNetCore;
using Yi.Framework.Abstractions;
using Yi.Framework.Core.Entities;

namespace Yi.System.Domains;

public class UserManager : BaseDomain
{
    private readonly ISqlSugarRepository<UserEntity> _userRepository;
    private readonly ISqlSugarRepository<UserPostEntity> _userPostRepository;
    private readonly ISqlSugarRepository<UserRoleEntity> _userRoleRepository;
    private readonly ISqlSugarRepository<RoleEntity> _roleRepository;

    private static readonly string[] ForbiddenNames = ["admin", "cc"];

    public UserManager(
        ISqlSugarRepository<UserEntity> userRepository,
        ISqlSugarRepository<UserRoleEntity> userRoleRepository,
        ISqlSugarRepository<UserPostEntity> userPostRepository,
        ISqlSugarRepository<RoleEntity> roleRepository)
    {
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

        // if (userEntity.EncryPassword?.Password.Length < 6)
        // {
        //     throw Oops.Oh(SystemErrorCodes.UserPasswordTooShort);
        // }

        if (userEntity.Phone is not null)
        {
            if (await _userRepository.IsAnyAsync(x => x.Phone == userEntity.Phone))
            {
                throw Oops.Oh(SystemErrorCodes.UserPhoneRepeated);
            }
        }

        if (await _userRepository.IsAnyAsync(x => x.UserName == userEntity.UserName))
        {
            throw Oops.Oh(SystemErrorCodes.UserNameRepeated);
        }

        await _userRepository.InsertReturnEntityAsync(userEntity);
    }

    public async Task UpdateAsync(UserEntity userEntity)
    {
        ValidateUserName(userEntity);

        if (userEntity.Phone is not null)
        {
            if (await _userRepository.IsAnyAsync(x => x.Phone == userEntity.Phone && !x.Id.Equals(userEntity.Id)))
            {
                throw Oops.Oh(SystemErrorCodes.UserPhoneRepeated);
            }
        }

        if (await _userRepository.IsAnyAsync(x => x.UserName!.Equals(userEntity.UserName) && !x.Id.Equals(userEntity.Id)))
        {
            throw Oops.Oh(SystemErrorCodes.UserNameRepeated);
        }

        await _userRepository.UpdateAsync(userEntity);
    }

    public async Task SetDefaultRoleAsync(Guid userId)
    {
        var role = await _roleRepository.GetFirstAsync(x => x.RoleCode == "default");
        if (role is not null)
        {
            await GiveUserSetRoleAsync(new List<Guid> { userId }, new List<Guid> { role.Id });
        }
    }

    private void ValidateUserName(UserEntity input)
    {
        if (ForbiddenNames.Contains(input.UserName))
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
}