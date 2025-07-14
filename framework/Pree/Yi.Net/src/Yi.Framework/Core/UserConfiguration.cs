using Yi.Framework.Core.Entities;

namespace Yi.Framework.Core;

public class UserConfiguration
{
    public UserEntity User { get; set; } = new();

    public List<RoleEntity> Roles { get; set; } = new();

    public List<MenuEntity> Menus { get; set; } = new();

    public List<string> Permissions { get; set; } = new();

    public bool IsAdmin() => Roles.Any(r => r.RoleCode == "admin");
    
    public bool HasPermission(string permission) => Permissions.Any(p => p == permission);
}