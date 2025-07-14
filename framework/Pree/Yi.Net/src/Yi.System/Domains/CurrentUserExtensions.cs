using Newtonsoft.Json;
using Yi.AspNetCore.Security;

namespace Yi.System.Domains;

public static class CurrentUserExtensions
{
    // /// <summary>
    // ///     获取用户权限codes
    // /// </summary>
    // /// <param name="currentUser"></param>
    // /// <returns></returns>
    // public static List<string> GetPermissions(this ICurrentUser currentUser)
    // {
    //     return currentUser.FindClaims(TokenClaimConst.Permission).Select(x => x.Value).ToList();
    // }
    
    /// <summary>
    ///     是否管理员
    /// </summary>
    /// <param name="currentUser"></param>
    /// <returns></returns>
    public static bool IsAdmin(this ICurrentUser currentUser)
    {
        return currentUser.Roles.Any(role => role.Equals("admin"));
    }
    
    /// <summary>
    ///     获取用户权限岗位id
    /// </summary>
    /// <param name="currentUser"></param>
    /// <returns></returns>
    public static Guid? GetDeptId(this ICurrentUser currentUser)
    {
        var deptIdOrNull = currentUser.FindClaims(ClaimsIdentityTypes.Dept).Select(x => x.Value).FirstOrDefault();
        return deptIdOrNull is null ? null : Guid.Parse(deptIdOrNull);
    }

    public static List<RoleScope>? GetRoleScope(this ICurrentUser currentUser)
    {
        var roleOrNull = currentUser.FindClaims(ClaimsIdentityTypes.RoleScope).Select(x => x.Value).FirstOrDefault();
        return roleOrNull is null ? null : JsonConvert.DeserializeObject<List<RoleScope>>(roleOrNull);
    }
    
    public static bool IsRefreshToken(this ICurrentUser currentUser)
    {
        var refreshOrNull = currentUser.FindClaims(ClaimsIdentityTypes.Refresh).Select(x => x.Value).FirstOrDefault();
        return refreshOrNull is not null && bool.Parse(refreshOrNull);
    }
}