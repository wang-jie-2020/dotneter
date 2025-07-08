namespace Yi.System.Services.Dtos;

public class RegisterInput
{
    /// <summary>
    ///     账号
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    ///     密码
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    ///     唯一标识码
    /// </summary>
    public string? Uuid { get; set; }

    /// <summary>
    ///     电话
    /// </summary>
    public long Phone { get; set; }

    /// <summary>
    ///     验证码
    /// </summary>
    public string? Code { get; set; }
}