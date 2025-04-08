using Yi.Sys.Domains.Infra.Entities;

namespace Yi.Sys.Services.Infra.Dtos;

public class UserGetOutputDto 
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; }
    
    public int? Age { get; set; }
    
    public string UserName { get; set; } = string.Empty;
    
    public string? Icon { get; set; }
    
    public string? Nick { get; set; }
    
    public string? Email { get; set; }
    
    public string? Ip { get; set; }
    
    public string? Address { get; set; }
    
    public long? Phone { get; set; }
    
    public string? Introduction { get; set; }
    
    public string? Remark { get; set; }
    
    public SexEnum Sex { get; set; } = SexEnum.Unknown;
    
    public bool State { get; set; }
    
    public DateTime CreationTime { get; set; }

    public Guid? DeptId { get; set; }

    public DeptGetOutputDto? Dept { get; set; }

    public List<PostDto>? Posts { get; set; }

    public List<RoleDto>? Roles { get; set; }
}