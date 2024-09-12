namespace Yi.Infra.Rbac.Dtos;

/// <summary>
///     Role输入创建对象
/// </summary>
public class RoleCreateInput
{
    public string? RoleName { get; set; }
    public string? RoleCode { get; set; }
    public string? Remark { get; set; }
    public DataScopeEnum DataScope { get; set; } = DataScopeEnum.ALL;
    public bool State { get; set; } = true;

    public int OrderNum { get; set; }

    public List<Guid> DeptIds { get; set; }

    public List<Guid> MenuIds { get; set; }
}