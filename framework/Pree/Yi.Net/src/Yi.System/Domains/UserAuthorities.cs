using Yi.System.Entities;

namespace Yi.System.Domains;

public class UserAuthorities
{
    public UserEntity User { get; set; } = new();

    public List<RoleEntity> Roles { get; set; } = new();

    public List<MenuEntity> Menus { get; set; } = new();

    public List<string> Permissions { get; set; } = new();

    public bool IsAdmin() => Roles.Any(r => r.RoleCode == AccountConst.Admin);
    
    public bool HasPermission(string permission) => Permissions.Any(p => p == permission);
}