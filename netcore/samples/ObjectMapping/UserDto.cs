namespace ObjectMapping;

public class UserDto
{
    public string Name { get; set; }
    
    public List<RoleDto> Roles { get; set; }
}