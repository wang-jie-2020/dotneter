namespace Yi.Infra.Rbac.Dtos.User;

public class UserGetListInputVo : PagedInfraInput
{
    public string? Name { get; set; }
    public string? UserName { get; set; }
    public long? Phone { get; set; }

    public bool? State { get; set; }

    public Guid? DeptId { get; set; }

    public string? Ids { get; set; }
}