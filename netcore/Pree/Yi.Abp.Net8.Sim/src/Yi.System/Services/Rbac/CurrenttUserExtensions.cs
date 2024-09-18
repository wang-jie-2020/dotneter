using Newtonsoft.Json;
using Volo.Abp.Users;
using Yi.AspNetCore.Permissions;

namespace Yi.System.Services.Rbac;

public static class CurrenttUserExtensions
{
    /// <summary>
    ///     获取用户权限codes
    /// </summary>
    /// <param name="currentUser"></param>
    /// <returns></returns>
    public static List<string> GetPermissions(this ICurrentUser currentUser)
    {
        return currentUser.FindClaims(TokenClaimConst.Permission).Select(x => x.Value).ToList();
    }

    /// <summary>
    ///     获取用户权限岗位id
    /// </summary>
    /// <param name="currentUser"></param>
    /// <returns></returns>
    public static Guid? GetDeptId(this ICurrentUser currentUser)
    {
        var deptIdOrNull = currentUser.FindClaims(TokenClaimConst.DeptId).Select(x => x.Value).FirstOrDefault();
        return deptIdOrNull is null ? null : Guid.Parse(deptIdOrNull);
    }

    public static List<RoleTokenInfoModel>? GetRoleInfo(this ICurrentUser currentUser)
    {
        var roleOrNull = currentUser.FindClaims(TokenClaimConst.RoleInfo).Select(x => x.Value).FirstOrDefault();
        return roleOrNull is null ? null : JsonConvert.DeserializeObject<List<RoleTokenInfoModel>>(roleOrNull);
    }

    public static bool IsRefreshToken(this ICurrentUser currentUser)
    {
        var refreshOrNull = currentUser.FindClaims(TokenClaimConst.Refresh).Select(x => x.Value).FirstOrDefault();
        return refreshOrNull is null ? false : bool.Parse(refreshOrNull);
    }
}