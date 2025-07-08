using Yi.System.Entities;

namespace Yi.System.Services.Dtos;

public class UserDetailDto : UserDto
{
    public DeptDto? Dept { get; set; }

    public List<PostDto>? Posts { get; set; }

    public List<RoleDto>? Roles { get; set; }
}