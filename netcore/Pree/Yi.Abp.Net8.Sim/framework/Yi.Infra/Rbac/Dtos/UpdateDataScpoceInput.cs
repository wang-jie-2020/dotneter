namespace Yi.Infra.Rbac.Dtos;

public class UpdateDataScpoceInput
{
    public Guid RoleId { get; set; }

    public List<Guid>? DeptIds { get; set; }

    public DataScopeEnum DataScope { get; set; }
}