using Volo.Abp.Application.Dtos;
using Yi.Infra.Rbac.Entities;

namespace Yi.Infra.Rbac.Dtos;

public class UserGetListOutputDto : EntityDto<Guid>
{
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
    
    public Guid? DeptId { get; set; }
    
    public DateTime CreationTime { get; set; } = DateTime.Now;
    
    public Guid? CreatorId { get; set; }

    public bool State { get; set; }
    
    public string DeptName { get; set; }
}