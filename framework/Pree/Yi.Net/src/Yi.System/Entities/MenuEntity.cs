namespace Yi.System.Entities;

/// <summary>
///     菜单表
/// </summary>
[SugarTable("Sys_Menu")]
public class MenuEntity : BizEntity<Guid>
{
    public MenuEntity()
    {
    }

    public MenuEntity(Guid id)
    {
        Id = id;
        ParentId = Guid.Empty;
    }

    public MenuEntity(Guid id, Guid parentId)
    {
        Id = id;
        ParentId = parentId;
    }
    
    /// <summary>
    ///     菜单名
    /// </summary>
    public string MenuName { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    [SugarColumn(ColumnName = "MenuType")]
    public MenuTypeEnum MenuType { get; set; } = MenuTypeEnum.Menu;

    /// <summary>
    /// </summary>
    [SugarColumn(ColumnName = "PermissionCode")]
    public string? PermissionCode { get; set; }

    /// <summary>
    /// </summary>
    [SugarColumn(ColumnName = "ParentId")]
    public Guid ParentId { get; set; }

    /// <summary>
    ///     菜单图标
    /// </summary>
    [SugarColumn(ColumnName = "MenuIcon")]
    public string? MenuIcon { get; set; }

    /// <summary>
    ///     菜单组件路由
    /// </summary>
    [SugarColumn(ColumnName = "Router")]
    public string? Router { get; set; }

    /// <summary>
    ///     是否为外部链接
    /// </summary>
    [SugarColumn(ColumnName = "IsLink")]
    public bool IsLink { get; set; }

    /// <summary>
    ///     是否缓存
    /// </summary>
    [SugarColumn(ColumnName = "IsCache")]
    public bool IsCache { get; set; }

    /// <summary>
    ///     是否显示
    /// </summary>
    [SugarColumn(ColumnName = "IsShow")]
    public bool IsShow { get; set; } = true;

    /// <summary>
    ///     描述
    /// </summary>
    [SugarColumn(ColumnName = "Remark")]
    public string? Remark { get; set; }

    /// <summary>
    ///     组件路径
    /// </summary>
    [SugarColumn(ColumnName = "Component")]
    public string? Component { get; set; }

    /// <summary>
    ///     路由参数
    /// </summary>
    [SugarColumn(ColumnName = "Query")]
    public string? Query { get; set; }

    [SugarColumn(IsIgnore = true)] 
    public List<MenuEntity>? Children { get; set; }
    
    /// <summary>
    ///     排序
    /// </summary>
    public int OrderNum { get; set; } = 0;
    
    /// <summary>
    ///     状态
    /// </summary>
    public bool State { get; set; }
}