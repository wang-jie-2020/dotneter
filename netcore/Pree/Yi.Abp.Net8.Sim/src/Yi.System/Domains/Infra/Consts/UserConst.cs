namespace Yi.Sys.Domains.Infra.Consts;

/// <summary>
///     常量定义
/// </summary>
public class UserConst
{
    public const string Signup_Forbidden = "暂未开放注册功能";
    
    public const string Login_Error = "用户名或密码错误";
    public const string Login_Not_Exist = "用户名不存在";
    
    public const string VerificationCode_Invalid = "Invalid_VerificationCode";
    public const string VerificationCode_TooMuch = "已发送过验证码，10分钟后可重试";
    public const string VerificationCode_NotMatched = "验证码错误";
    
    public const string User_Exist = "用户已经存在";
    public const string User_Not_Exist = "用户不存在";
    public const string User_State = "该用户已被禁用，请联系管理员进行恢复";
    public const string User_No_Role = "该用户分配无任何角色";
    public const string User_No_Permission = "该用户分配无任何权限";
    
    public const string User_Name_Not_Allowed = "用户名被禁止";
    public const string User_Name_Length = "账号名需大于等于2位！";
    public const string User_Name_Invalid = "用户名不能包含除【字母】与【数字】的其他字符";
    
    public const string User_Password_NotMatched = "密码错误";
    public const string User_Password_Length = "密码长度需大于等于6位";

    public const string User_Phone_Repeat = "手机号已注册";
    public const string User_Phone_Invalid = "手机号码格式错误";
    
    public const string Tenant_Exist = "租户已经存在";
    public const string Tenant_Repeat = "租户名已经存在";
    
    //子租户管理员
    public const string Admin = "cc";
    public const string AdminRolesCode = "admin";
    public const string AdminPermissionCode = "*:*:*";

    //租户管理员
    public const string TenantAdmin = "ccadmin";
    
    public const string DefaultRoleCode = "default";
}