using Yi.Infra.Rbac.Dtos;

namespace Yi.Infra.Rbac.Managers;

public class UserRoleMenuQueryEventArgs
{
    public UserRoleMenuQueryEventArgs()
    {
    }

    public UserRoleMenuQueryEventArgs(params Guid[] userIds)
    {
        UserIds.AddRange(userIds.ToList());
    }

    public List<Guid> UserIds { get; set; } = new();

    public List<UserRoleMenuDto>? Result { get; set; }
}