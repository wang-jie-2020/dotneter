namespace Yi.Framework.Options;

public class RbacOptions
{
    /// <summary>
    ///     是否开启登录验证码
    /// </summary>
    public bool EnableCaptcha { get; set; } = true;

    /// <summary>
    ///     是否开启用户注册功能
    /// </summary>
    public bool EnableRegister { get; set; } = false;
}