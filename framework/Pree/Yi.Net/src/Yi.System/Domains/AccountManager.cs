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
using Yi.System.Domains.Entities;
using Yi.System.Domains.Repositories;
using Yi.System.Monitor.Entities;
using Yi.System.Options;
using Yi.System.Services.Dtos;

namespace Yi.System.Domains;

public class AccountManager : BaseDomain
{
    private readonly IUserRepository _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtOptions _jwtOptions;
    private readonly RefreshJwtOptions _refreshJwtOptions;
    private readonly UserManager _userManager;
    private readonly ISqlSugarRepository<RoleEntity> _roleRepository;
    private readonly ISqlSugarRepository<LoginLogEntity> _loginLogRepository;
    
    public AccountManager(IUserRepository repository
        , IHttpContextAccessor httpContextAccessor
        , IOptions<JwtOptions> jwtOptions
        , IOptions<RefreshJwtOptions> refreshJwtOptions
        , UserManager userManager
        , ISqlSugarRepository<RoleEntity> roleRepository
        , ISqlSugarRepository<LoginLogEntity> loginLogRepository)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
        _jwtOptions = jwtOptions.Value;
        _refreshJwtOptions = refreshJwtOptions.Value;
        _userManager = userManager;
        _roleRepository = roleRepository;
        _loginLogRepository = loginLogRepository;
    }

    /// <summary>
    ///     根据用户id获取token
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [OperLog("生成token", OperLogEnum.Auth)]
    public async Task<string> GetTokenByUserIdAsync(Guid userId)
    {
        //获取用户信息
        var userInfo = await _userManager.GetInfoAsync(userId);

        //判断用户状态
        if (userInfo.User.State == false)
        {
            throw Oops.Oh(SystemErrorCodes.UserInactive);
        }

        if (userInfo.RoleCodes.Count == 0)
        {
            throw Oops.Oh(SystemErrorCodes.UserNoRole);
        }

        if (userInfo.PermissionCodes.Count() == 0)
        {
            throw Oops.Oh(SystemErrorCodes.UserNoPermission);
        }

        //这里抛出一个登录的事件,也可以在全部流程走完，在应用层组装
        if (_httpContextAccessor.HttpContext is not null)
        {
            var loginEto = new LoginEventArgs().GetInfoByHttpContext(_httpContextAccessor.HttpContext);
            loginEto.UserName = userInfo.User.UserName;
            loginEto.UserId = userInfo.User.Id;

            var loginLogEntity = loginEto.Adapt<LoginLogEntity>();
            loginLogEntity.LogMsg = loginEto.UserName + "登录系统";
            loginLogEntity.LoginUser = loginEto.UserName;
            loginLogEntity.CreatorId = loginEto.UserId;
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
            new(TokenClaimConst.Refresh, "true")
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
        var user = await _repository.GetByIdAsync(userId);
        if (!user.JudgePassword(oldPassword))
        {
            throw Oops.Oh(SystemErrorCodes.GivenPasswordNotMatched);
        }

        user.EncryPassword.Password = newPassword;
        user.BuildPassword();
        await _repository.UpdateAsync(user);
    }

    /// <summary>
    ///     重置密码
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<bool> RestPasswordAsync(Guid userId, string password)
    {
        var user = await _repository.GetByIdAsync(userId);
        // EntityHelper.TrySetId(user, () => GuidGenerator.Create(), true);
        user.EncryPassword.Password = password;
        user.BuildPassword();
        return await _repository.UpdateAsync(user);
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
    public async Task<bool> ExistAsync(string userName, Action<UserEntity> userAction = null)
    {
        var user = await _repository.GetFirstAsync(u => u.UserName == userName && u.State == true);
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
    /// <param name="dto"></param>
    /// <returns></returns>
    public List<KeyValuePair<string, string>> UserInfoToClaim(UserRoleMenuDto dto)
    {
        var claims = new List<KeyValuePair<string, string>>();
        AddToClaim(claims, ClaimsIdentityTypes.UserId, dto.User.Id.ToString());
        AddToClaim(claims, ClaimsIdentityTypes.UserName, dto.User.UserName);

        if (dto.User.DeptId is not null)
        {
            AddToClaim(claims, TokenClaimConst.DeptId, dto.User.DeptId.ToString());
        }

        if (dto.Roles.Count > 0)
        {
            AddToClaim(claims, TokenClaimConst.RoleInfo,
                JsonConvert.SerializeObject(dto.Roles.Select(x => new RoleTokenInfo
                    { Id = x.Id, DataScope = x.DataScope })));
        }

        if (dto.Roles.Any(f => f.Equals(AccountConst.AdminRole)))
        {
            AddToClaim(claims, TokenClaimConst.Roles, AccountConst.AdminRole);
        }
        else
        {
            dto.RoleCodes?.ForEach(role => AddToClaim(claims, ClaimsIdentityTypes.Role, role));
        }

        return claims;
    }

    private void AddToClaim(List<KeyValuePair<string, string>> claims, string key, string value)
    {
        claims.Add(new KeyValuePair<string, string>(key, value));
    }
}