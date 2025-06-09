namespace Yi.AspNetCore.Core;

[Serializable]
public class PagedInput
{
    public int PageNum { get; set; }
    
    public int PageSize { get; set; } = 10;
    
    public string? Sorting { get; set; }
    
    /// <summary>
    ///     查询开始时间条件
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    ///     查询结束时间条件
    /// </summary>
    public DateTime? EndTime { get; set; }
}