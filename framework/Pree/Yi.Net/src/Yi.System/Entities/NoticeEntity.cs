namespace Yi.System.Entities;

[SugarTable("Sys_Notice")]
public class NoticeEntity : BizEntity<Guid>
{
    /// <summary>
    ///     公告标题
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    ///     类型
    /// </summary>
    public NoticeTypeEnum Type { get; set; }

    /// <summary>
    ///     内容
    /// </summary>
    [SugarColumn(ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string Content { get; set; }

    public int OrderNum { get; set; }
    
    public bool State { get; set; }
}