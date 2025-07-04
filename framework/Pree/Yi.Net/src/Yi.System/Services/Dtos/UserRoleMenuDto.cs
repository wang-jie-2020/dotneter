using Newtonsoft.Json;

namespace Yi.System.Services.Dtos;

public class UserRoleMenuDto
{
    public UserDto User { get; set; } = new();
    
    public HashSet<RoleDto> Roles { get; set; } = new();
    
    public HashSet<MenuDto> Menus { get; set; } = new();

    public List<string> RoleCodes { get; set; } = new();
    
    public List<string> PermissionCodes { get; set; } = new();
}