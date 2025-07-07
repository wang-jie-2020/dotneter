using System.Security.Claims;

namespace Yi.AspNetCore.Security;

public class ClaimsIdentityTypes
{
    public static string UserName { get; } = ClaimTypes.Name;

    public static string UserId { get; } = ClaimTypes.NameIdentifier;

    public static string Role { get; } = ClaimTypes.Role;

    public static string Dept { get; } = "dept";

    public static string RoleScope { get; } = "role_scope";

    public static string Refresh { get; } = "refresh";
}