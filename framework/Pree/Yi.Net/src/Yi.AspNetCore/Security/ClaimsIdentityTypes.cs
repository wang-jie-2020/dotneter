namespace Yi.AspNetCore.Security;

public class ClaimsIdentityTypes
{
    public static string UserName { get; } = "name";

    public static string UserId { get; } = "sub";

    public static string Role { get; } = "role";
}
