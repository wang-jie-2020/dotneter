namespace Yi.Framework.Core.Helper;

public static class IdHelper
{
    public static dynamic[] ToDynamicArray(this IEnumerable<long> ids)
    {
        return ids.Select(id => (dynamic)id).ToArray();
    }
}