namespace Yi.Sys.Services.Infra.Impl;

/// <summary>
///     常量定义
/// </summary>
public class UserConst
{
    public const string Login_Error = "登录失败！用户名或密码错误！";
    public const string Login_User_No_Exist = "登录失败！用户名不存在！";
    public const string Login_Passworld_Error = "密码为空，添加失败！";
    public const string Create_Passworld_Error = "密码格式错误，长度需大于等于6位";
    public const string User_Exist = "用户已经存在，创建失败！";
    public const string State_Is_State = "该用户已被禁用，请联系管理员进行恢复";
    public const string No_Permission = "登录禁用！该用户分配无任何权限，无意义登录！";
    public const string No_Role = "登录禁用！该用户分配无任何角色，无意义登录！";
    public const string Name_Not_Allowed = "用户名被禁止";
    public const string Phone_Repeat = "手机号已注册";
    public const string Phone_Invalid = "手机号码格式错误";
    
    public const string Tenant_Exist = "创建失败，当前租户已存在";
    public const string Tenant_Repeat = "更新后租户名已经存在";
    
    public const string Invalid_VerificationCode = "Invalid_VerificationCode";

    public const string Name_Length = "账号名需大于等于2位！";
    public const string Name_Invalid = "用户名不能包含除【字母】与【数字】的其他字符";

    public const string Name_Repeat = "用户已经存在";

    public const string User_Not_Exist = "用户未存在";

    public const string Signup_Forbidden = "该系统暂未开放注册功能";

    public const string VerificationCode_TooMuch = "已发送过验证码，10分钟后可重试";
    public const string VerificationCode_NotMatched = "已发送过验证码，10分钟后可重试";

    public const string Password_Error = "密码错误";
    
    //子租户管理员
    public const string Admin = "cc";
    public const string AdminRolesCode = "admin";
    public const string AdminPermissionCode = "*:*:*";

    //租户管理员
    public const string TenantAdmin = "ccadmin";
    public const string TenantAdminPermissionCode = "*";

    public const string DefaultRoleCode = "default";
    public const string CommonRoleName = "common";
}