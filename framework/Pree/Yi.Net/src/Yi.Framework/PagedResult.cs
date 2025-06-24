namespace Yi.Framework;

[Serializable]
public class PagedResult<T>
{
    public IReadOnlyList<T> Items
    {
        get { return _items ?? (_items = new List<T>()); }
        set { _items = value; }
    }
    private IReadOnlyList<T>? _items;
    
    public long TotalCount { get; set; } 
    
    public PagedResult()
    {

    }
    
    public PagedResult(long totalCount, IReadOnlyList<T> items)
    {
        TotalCount = totalCount;
        Items = items;
    }
}