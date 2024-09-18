namespace Yi.System.Rbac.Dtos;

public class UpdateDataScopeInput
{
    public Guid RoleId { get; set; }

    public List<Guid>? DeptIds { get; set; }

    public DataScopeEnum DataScope { get; set; }
}