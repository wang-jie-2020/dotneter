namespace Yi.Framework.Core;

[Serializable]
public class PagedInput
{
    public int PageNum { get; set; }
    
    public int PageSize { get; set; } = 10;
    
    public string? Sorting { get; set; }
    
    public DateTime? StartTime { get; set; }
    
    public DateTime? EndTime { get; set; }
}