namespace Yi.System.Services.Dtos;

/// <summary>
///     Role输入创建对象
/// </summary>
public class RoleInput
{
    public string? RoleName { get; set; }
    
    public string? RoleCode { get; set; }
    
    public string? Remark { get; set; }
    
    public DataScopeEnum DataScope { get; set; } = DataScopeEnum.ALL;
    
    public bool State { get; set; } = true;

    public int OrderNum { get; set; }

    public List<Guid> MenuIds { get; set; }
}