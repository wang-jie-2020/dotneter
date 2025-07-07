using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Yi.AspNetCore;
using Yi.AspNetCore.Security;
using Yi.Framework.Abstractions;
using Yi.Framework.Loggings;
using Yi.Framework.Utils;
using Yi.System.Entities;
using Yi.System.Monitor.Entities;
using Yi.System.Options;

namespace Yi.System.Domains;

public class AccountManager : BaseDomain
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtOptions _jwtOptions;
    private readonly RefreshJwtOptions _refreshJwtOptions;
    private readonly UserManager _userManager;
    private readonly ISqlSugarRepository<UserEntity> _userRepository;
    private readonly ISqlSugarRepository<LoginLogEntity> _loginLogRepository;

    public AccountManager(
        IHttpContextAccessor httpContextAccessor,
        IOptions<JwtOptions> jwtOptions,
        IOptions<RefreshJwtOptions> refreshJwtOptions,
        UserManager userManager,
        ISqlSugarRepository<UserEntity> userRepository,
        ISqlSugarRepository<LoginLogEntity> loginLogRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtOptions = jwtOptions.Value;
        _refreshJwtOptions = refreshJwtOptions.Value;
        _userManager = userManager;
        _userRepository = userRepository;
        _loginLogRepository = loginLogRepository;
    }

    /// <summary>
    ///     根据用户id获取token
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [OperLog("生成token", OperLogEnum.Auth)]
    public async Task<string> CreateTokenAsync(Guid userId)
    {
        //获取用户信息
        var userInfo = await _userManager.GetInfoAsync(userId);

        //判断用户状态
        if (userInfo.User.State == false)
        {
            throw Oops.Oh(SystemErrorCodes.UserInactive);
        }

        if (userInfo.Roles.Count == 0)
        {
            throw Oops.Oh(SystemErrorCodes.UserNoRole);
        }

        if (userInfo.Permissions.Count() == 0)
        {
            throw Oops.Oh(SystemErrorCodes.UserNoPermission);
        }

        //这里抛出一个登录的事件,也可以在全部流程走完，在应用层组装
        if (_httpContextAccessor.HttpContext is not null)
        {
            var loginEvent = new LoginEventArgs().GetInfoByHttpContext(_httpContextAccessor.HttpContext);
            loginEvent.UserName = userInfo.User.UserName;
            loginEvent.UserId = userInfo.User.Id;

            var loginLogEntity = loginEvent.Adapt<LoginLogEntity>();
            loginLogEntity.LogMsg = loginEvent.UserName + "登录系统";
            loginLogEntity.LoginUser = loginEvent.UserName;
            loginLogEntity.CreatorId = loginEvent.UserId;
            await _loginLogRepository.InsertAsync(loginLogEntity);
        }

        var accessToken = CreateToken(UserInfoToClaim(userInfo));
        return accessToken;
    }

    public string CreateRefreshToken(Guid userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_refreshJwtOptions.SecurityKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //添加用户id，及刷新token的标识
        var claims = new List<Claim>
        {
            new(ClaimsIdentityTypes.UserId, userId.ToString()),
            new(ClaimsIdentityTypes.Refresh, "true")
        };

        var token = new JwtSecurityToken(
            _refreshJwtOptions.Issuer,
            _refreshJwtOptions.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_refreshJwtOptions.ExpiresMinuteTime),
            notBefore: DateTime.Now,
            signingCredentials: creds);

        var returnToken = new JwtSecurityTokenHandler().WriteToken(token);

        return returnToken;
    }

    /// <summary>
    ///     登录校验
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="userAction"></param>
    /// <returns></returns>
    public async Task LoginValidationAsync(string userName, string password,
        Action<UserEntity> userAction = null)
    {
        var user = new UserEntity();
        if (await ExistAsync(userName, o => user = o))
        {
            if (userAction is not null)
            {
                userAction.Invoke(user);
            }

            // 防止有权限修改,放在登出只能cover部分场景
            await _userManager.RemoveCacheAsync(user.Id);

            if (user.EncryPassword.Password == MD5Helper.SHA2Encode(password, user.EncryPassword.Salt))
            {
                return;
            }

            throw Oops.Oh(SystemErrorCodes.GivenPasswordNotMatched);
        }

        throw Oops.Oh(SystemErrorCodes.GivenNameNotExist);
    }

    /// <summary>
    ///     更新密码
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="newPassword"></param>
    /// <param name="oldPassword"></param>
    /// <returns></returns>
    /// <exception cref="UserFriendlyException"></exception>
    public async Task UpdatePasswordAsync(Guid userId, string newPassword, string oldPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (!user.JudgePassword(oldPassword))
        {
            throw Oops.Oh(SystemErrorCodes.GivenPasswordNotMatched);
        }

        user.EncryPassword.Password = newPassword;
        user.BuildPassword();
        await _userRepository.UpdateAsync(user);
    }

    /// <summary>
    ///     重置密码
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<bool> RestPasswordAsync(Guid userId, string password)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        // EntityHelper.TrySetId(user, () => GuidGenerator.Create(), true);
        user.EncryPassword.Password = password;
        user.BuildPassword();
        return await _userRepository.UpdateAsync(user);
    }

    /// <summary>
    ///     注册用户，创建用户之后设置默认角色
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <param name="phone"></param>
    /// <returns></returns>
    public async Task RegisterAsync(string userName, string password, long phone)
    {
        var user = new UserEntity(userName, password, phone);
        await _userManager.CreateAsync(user);
        await _userManager.SetDefaultRoleAsync(user.Id);
    }

    /// <summary>
    ///     创建令牌
    /// </summary>
    /// <param name="kvs"></param>
    /// <returns></returns>
    private string CreateToken(List<KeyValuePair<string, string>> kvs)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = kvs.Select(x => new Claim(x.Key, x.Value.ToString())).ToList();
        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtOptions.ExpiresMinuteTime),
            notBefore: DateTime.Now,
            signingCredentials: creds);
        var returnToken = new JwtSecurityTokenHandler().WriteToken(token);

        return returnToken;
    }

    /// <summary>
    ///     判断账户合法存在
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="userAction"></param>
    /// <returns></returns>
    private async Task<bool> ExistAsync(string userName, Action<UserEntity> userAction = null)
    {
        var user = await _userRepository.GetFirstAsync(u => u.UserName == userName && u.State == true);
        if (userAction is not null)
        {
            userAction.Invoke(user);
        }

        //这里为了兼容解决数据库开启了大小写不敏感问题,还要将用户名进行二次校验
        if (user != null && user.UserName == userName)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    ///     令牌转换
    /// </summary>
    /// <param name="authorities"></param>
    /// <returns></returns>
    private List<KeyValuePair<string, string>> UserInfoToClaim(UserAuthorities authorities)
    {
        var claims = new List<KeyValuePair<string, string>>();
        AddToClaim(claims, ClaimsIdentityTypes.UserId, authorities.User.Id.ToString());
        AddToClaim(claims, ClaimsIdentityTypes.UserName, authorities.User.UserName);

        if (authorities.User.DeptId is not null)
        {
            AddToClaim(claims, ClaimsIdentityTypes.Dept, authorities.User.DeptId.ToString());
        }

        if (authorities.Roles.Count > 0)
        {
            AddToClaim(claims, ClaimsIdentityTypes.Role,
                JsonConvert.SerializeObject(authorities.Roles.Select(x => x.RoleCode)));

            AddToClaim(claims, ClaimsIdentityTypes.RoleScope,
                JsonConvert.SerializeObject(authorities.Roles.Select(x => new RoleTokenInfo
                    { Id = x.Id, DataScope = x.DataScope })));
        }

        return claims;
    }

    private void AddToClaim(List<KeyValuePair<string, string>> claims, string key, string value)
    {
        claims.Add(new KeyValuePair<string, string>(key, value));
    }
}