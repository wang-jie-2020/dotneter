namespace Yi.AspNetCore.Core;

[Serializable]
public class PagedResult<T>
{
    public IReadOnlyList<T> Items
    {
        get { return _items ?? (_items = new List<T>()); }
        set { _items = value; }
    }
    private IReadOnlyList<T>? _items;

    /// <inheritdoc />
    public long TotalCount { get; set; } //TODO: Can be a long value..?

    /// <summary>
    /// Creates a new <see cref="PagedResult{T}"/> object.
    /// </summary>
    public PagedResult()
    {

    }

    /// <summary>
    /// Creates a new <see cref="PagedResult{T}"/> object.
    /// </summary>
    /// <param name="totalCount">Total count of Items</param>
    /// <param name="items">List of items in current page</param>
    public PagedResult(long totalCount, IReadOnlyList<T> items)
    {
        TotalCount = totalCount;
        Items = items;
    }
}