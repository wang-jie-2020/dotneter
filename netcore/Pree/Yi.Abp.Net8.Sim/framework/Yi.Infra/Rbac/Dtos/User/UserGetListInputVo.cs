using Yi.Framework.Ddd.Application;

namespace Yi.Infra.Rbac.Dtos.User;

public class UserGetListInputVo : PagedAllResultRequestDto
{
    public string? Name { get; set; }
    public string? UserName { get; set; }
    public long? Phone { get; set; }

    public bool? State { get; set; }

    public Guid? DeptId { get; set; }

    public string? Ids { get; set; }
}