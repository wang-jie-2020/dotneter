using Yi.System.Domains.System.Entities;

namespace Yi.System.Services.System.Dtos;

/// <summary>
///     User输入创建对象
/// </summary>
public class UserCreateInput
{
    public string? Name { get; set; }
    
    public int? Age { get; set; }
    
    public string UserName { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public string? Icon { get; set; }
    
    public string? Nick { get; set; }
    
    public string? Email { get; set; }
    
    public string? Address { get; set; }
    
    public long? Phone { get; set; }
    
    public string? Introduction { get; set; }
    
    public string? Remark { get; set; }
    
    public SexEnum Sex { get; set; } = SexEnum.Unknown;
    
    public List<Guid>? RoleIds { get; set; }
    
    public List<Guid>? PostIds { get; set; }
    
    public Guid? DeptId { get; set; }
    
    public bool State { get; set; } = true;
}