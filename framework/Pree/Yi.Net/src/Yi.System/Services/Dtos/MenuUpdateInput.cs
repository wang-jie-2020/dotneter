using Yi.System.Domains.Entities;

namespace Yi.System.Services.Dtos;

public class MenuUpdateInput
{
    public Guid Id { get; set; }
    
    public Guid? CreatorId { get; set; }
    
    public bool State { get; set; }
    
    public string MenuName { get; set; } = string.Empty;
    
    public MenuTypeEnum MenuType { get; set; } = MenuTypeEnum.Menu;
    
    public string? PermissionCode { get; set; }
    
    public Guid ParentId { get; set; }
    
    public string? MenuIcon { get; set; }
    
    public string? Router { get; set; }
    
    public bool IsLink { get; set; }
    
    public bool IsCache { get; set; }
    
    public bool IsShow { get; set; } = true;
    
    public string? Remark { get; set; }
    
    public string? Component { get; set; }
    
    public string? Query { get; set; }
    
    public int OrderNum { get; set; }

    //public List<MenuEntity>? Children { get; set; }
}