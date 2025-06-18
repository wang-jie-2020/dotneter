using System.Security.Claims;

namespace Yi.AspNetCore.Security;

public class ClaimsIdentityTypes
{
    public static string UserName { get; } = ClaimTypes.Name;

    public static string UserId { get; } = ClaimTypes.NameIdentifier;

    public static string Role { get; } = ClaimTypes.Role;
}
