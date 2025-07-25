﻿using System.Text.RegularExpressions;
using Lazy.Captcha.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Volo.Abp.Uow;
using Yi.AspNetCore;
using Yi.AspNetCore.Extensions.Caching;
using Yi.AspNetCore.Security;
using Yi.Framework.Abstractions;
using Yi.Framework.Core;
using Yi.Framework.Core.Entities;
using Yi.Framework.Options;
using Yi.Framework.Utils;
using Yi.System;
using Yi.System.Domains;
using Yi.System.Services.Dtos;

namespace Yi.Admin.Controllers.System;

[ApiController]
[Route("api/system/account")]
public class AccountController : BaseController
{
    private readonly ICaptcha _captcha;
    private readonly ICurrentUser _currentUser;
    private readonly IDistributedCache _cache;
    private readonly RbacOptions _rbacOptions;
    private readonly AccountManager _accountManager;
    private readonly UserManager _userManager;
    private readonly IUserStore _userStore;
    private readonly ISqlSugarRepository<UserEntity> _userRepository;
    private readonly ISqlSugarRepository<MenuEntity> _menuRepository;

    public AccountController(
        ICaptcha captcha,
        ICurrentUser currentUser,
        IDistributedCache cache,
        IOptions<RbacOptions> options,
        AccountManager accountManager,
        UserManager userManager, 
        IUserStore userStore,
        ISqlSugarRepository<UserEntity> userRepository,
        ISqlSugarRepository<MenuEntity> menuRepository)
    {
        _captcha = captcha;
        _currentUser = currentUser;
        _cache = cache;
        _rbacOptions = options.Value;
        _accountManager = accountManager;
        _userManager = userManager;
        _userStore = userStore;
        _userRepository = userRepository;
        _menuRepository = menuRepository;
    }

#if DEBUG

    /// <summary>
    ///     登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("debugging")]
    public async Task<object> Debgging()
    {
        //校验
        UserEntity user = new();
        await _accountManager.LoginValidationAsync("cc", "123456", x => user = x);

        //获取token
        var accessToken = await _accountManager.CreateTokenAsync(user.Id);
        var refreshToken = _accountManager.CreateRefreshToken(user.Id);

        return new { Token = accessToken, RefreshToken = refreshToken };
    }

#endif

    /// <summary>
    ///     登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<object> PostLoginAsync([FromBody] LoginInput input)
    {
        if (string.IsNullOrEmpty(input.Password) || string.IsNullOrEmpty(input.UserName))
        {
            throw new ArgumentNullException();
        }

        //校验验证码
        ValidationImageCaptcha(input);

        //校验
        UserEntity user = new();
        await _accountManager.LoginValidationAsync(input.UserName, input.Password, x => user = x);

        //获取token
        var accessToken = await _accountManager.CreateTokenAsync(user.Id);
        var refreshToken = _accountManager.CreateRefreshToken(user.Id);

        return new { Token = accessToken, RefreshToken = refreshToken };
    }

    /// <summary>
    ///     生成验证码
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("captcha-image")]
    public async Task<CaptchaImage> GetCaptchaImageAsync()
    {
        var uuid = SequentialGuidGenerator.Create();
        var captcha = _captcha.Generate(uuid.ToString());
        return new CaptchaImage { Img = captcha.Bytes, Uuid = uuid };
    }

    /// <summary>
    ///     校验图片登录验证码,无需和账号绑定
    /// </summary>
    private void ValidationImageCaptcha(LoginInput input)
    {
        //登录不想要验证码 ，可不校验
        if (_rbacOptions.EnableCaptcha)
        {
            if (!_captcha.Validate(input.Uuid, input.Code))
            {
                throw Oops.Oh(SystemErrorCodes.VerificationCodeNotMatched);
            }
        }
    }

    /// <summary>
    ///     注册，需要验证码通过
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [UnitOfWork]
    [HttpPost("register")]
    public async Task PostRegisterAsync([FromBody] RegisterInput input)
    {
        if (_rbacOptions.EnableRegister == false)
        {
            throw Oops.Oh(SystemErrorCodes.SignupForbidden);
        }

        if (_rbacOptions.EnableCaptcha)
        {
            await ValidationPhoneCaptchaAsync(input);
        }

        await _accountManager.RegisterAsync(input.UserName, input.Password, input.Phone);
    }

    /// <summary>
    ///     查询已登录的账户信息，已缓存
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    public async Task<AccountInfo> GetAsync()
    {
        //通过鉴权jwt获取到用户的id
        var userId = _currentUser.Id;
        if (userId is null)
        {
            throw new ArgumentNullException(nameof(CurrentUser));
        }

        var user = await _userStore.GetInfoAsync(userId.Value);
        var info = new AccountInfo()
        {
            User = user.User.Adapt<UserDto>(),
            Permissions = user.Permissions,
            Roles = user.Roles.Select(p => p.RoleCode).ToList()
        };

        return info;
    }

    /// <summary>
    ///     重置密码
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("reset-password/{userId}")]
    public async Task<bool> RestPasswordAsync([FromRoute] Guid userId, [FromBody] ResetPasswordInput input)
    {
        if (string.IsNullOrEmpty(input.Password))
        {
            throw new ArgumentNullException(nameof(input.Password));
        }

        return await _accountManager.RestPasswordAsync(userId, input.Password);
    }

    /// <summary>
    ///     刷新token
    /// </summary>
    /// <param name="refresh_token"></param>
    /// <returns></returns>
    [Authorize(AuthenticationSchemes = "refresh")]
    [HttpPost("refresh")]
    public async Task<object> PostRefreshAsync([FromQuery] string? refresh_token)
    {
        var userId = CurrentUser.Id.Value;
        var accessToken = await _accountManager.CreateTokenAsync(userId);
        var refreshToken = _accountManager.CreateRefreshToken(userId);
        return new { Token = accessToken, RefreshToken = refreshToken };
    }

    /// <summary>
    ///     验证电话号码
    /// </summary>
    /// <param name="str_handset"></param>
    private async Task ValidationPhone(string str_handset)
    {
        var res = Regex.IsMatch(str_handset, @"^\d{11}$");
        if (res == false) throw Oops.Oh(SystemErrorCodes.UserPhoneInvalid);
        if (await _userRepository.IsAnyAsync(x => x.Phone.ToString() == str_handset))
        {
            throw Oops.Oh(SystemErrorCodes.UserPhoneRepeated);
        }
    }

    /// <summary>
    ///     注册 手机验证码
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("captcha-phone")]
    public async Task<object> PostCaptchaPhone([FromBody] CaptchaPhone input)
    {
        await ValidationPhone(input.Phone);
        var value = await _cache.GetAsync<CaptchaPhoneCacheItem>(CaptchaPhoneCacheItem.CalculateCacheKey(input.Phone));

        //防止暴刷
        if (value is not null)
        {
            throw Oops.Oh(SystemErrorCodes.VerificationCodeTooMuch)
                .WithData("Phone", input.Phone);
        }

        //生成一个4位数的验证码
        //发送短信，同时生成uuid
        ////key： 电话号码  value:验证码+uuid  
        var code = Guid.NewGuid().ToString().Substring(0, 4);
        var uuid = Guid.NewGuid();
        // await _aliyunManger.SendSmsAsync(input.Phone, code);

        await _cache.SetAsync(
            CaptchaPhoneCacheItem.CalculateCacheKey(input.Phone),
            new CaptchaPhoneCacheItem(code),
            new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(10) });

        return new
        {
            Uuid = uuid
        };
    }

    /// <summary>
    ///     校验电话验证码，需要与电话号码绑定
    /// </summary>
    private async Task ValidationPhoneCaptchaAsync(RegisterInput input)
    {
        var item = await _cache.GetAsync<CaptchaPhoneCacheItem>(CaptchaPhoneCacheItem.CalculateCacheKey(input.Phone.ToString()));
        if (item is not null && item.Code.Equals($"{input.Code}"))
        {
            //成功，需要清空
            await _cache.RemoveAsync(CaptchaPhoneCacheItem.CalculateCacheKey(input.Phone.ToString()));
            return;
        }

        throw Oops.Oh(SystemErrorCodes.VerificationCodeNotMatched);
    }

    /// <summary>
    ///     获取当前登录用户的前端路由
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("Vue3Router")]
    public async Task<List<Vue3Router>> GetVue3Router()
    {
        var userId = _currentUser.Id;
        if (_currentUser.Id is null)
        {
            throw new ArgumentNullException(nameof(CurrentUser));
        }

        var data = await _userStore.GetInfoAsync(userId!.Value);
        var menus = data.Menus.ToList();

        //将后端菜单转换成前端路由，组件级别需要过滤
        var routers = menus.Vue3RouterBuild();
        return routers;
    }

    /// <summary>
    ///     退出登录
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    public async Task<bool> PostLogout()
    {
        return true;
    }

    /// <summary>
    ///     更新密码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("password")]
    public async Task<bool> UpdatePasswordAsync([FromBody] ProfilePasswordInput input)
    {
        if (_currentUser.Id is null)
        {
            throw new ArgumentNullException(nameof(CurrentUser));
        }

        await _accountManager.UpdatePasswordAsync(_currentUser.Id ?? Guid.Empty, input.NewPassword, input.OldPassword);
        return true;
    }

    /// <summary>
    ///     更新头像
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("icon")]
    public async Task<bool> UpdateIconAsync([FromBody] ProfileIconInput input)
    {
        var entity = await _userRepository.GetByIdAsync(_currentUser.Id);
        entity.Icon = input.Icon;
        await _userRepository.UpdateAsync(entity);

        return true;
    }
}

public class CaptchaPhoneCacheItem
{
    public CaptchaPhoneCacheItem(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
    
    public static string CalculateCacheKey(string phone)
    {
        return $"Phone:{phone}";
    }
}