using Yi.Abp.Infra.Rbac.Dtos;

namespace Yi.Abp.Infra.Rbac.Etos
{
    public class UserRoleMenuQueryEventArgs
    {
        public UserRoleMenuQueryEventArgs() { }

        public UserRoleMenuQueryEventArgs(params Guid[] userIds)
        {
            UserIds.AddRange(userIds.ToList());
        }
        public List<Guid> UserIds { get; set; } = new List<Guid>();

        public List<UserRoleMenuDto>? Result { get; set; }
    }
}
