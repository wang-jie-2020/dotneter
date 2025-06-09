using Yi.System.Domains;

namespace Yi.System.Services.Dtos;

public class RoleUpdateInput
{
    public string? RoleName { get; set; }
    
    public string? RoleCode { get; set; }
    
    public string? Remark { get; set; }
    
    public DataScopeEnum DataScope { get; set; } = DataScopeEnum.ALL;
    
    public bool State { get; set; }

    public int OrderNum { get; set; }

    public List<Guid>? DeptIds { get; set; }

    public List<Guid>? MenuIds { get; set; }
}