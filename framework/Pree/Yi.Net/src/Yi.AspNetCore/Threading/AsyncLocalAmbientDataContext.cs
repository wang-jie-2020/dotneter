using System.Collections.Concurrent;
using Volo.Abp.DependencyInjection;

namespace Yi.AspNetCore.Threading;

public class AsyncLocalAmbientDataContext : IAmbientDataContext, ISingletonDependency
{
    private static readonly ConcurrentDictionary<string, AsyncLocal<object?>> AsyncLocalDictionary = new ConcurrentDictionary<string, AsyncLocal<object?>>();

    public void SetData(string key, object? value)
    {
        var asyncLocal = AsyncLocalDictionary.GetOrAdd(key, (k) => new AsyncLocal<object?>());
        asyncLocal.Value = value;
    }

    public object? GetData(string key)
    {
        var asyncLocal = AsyncLocalDictionary.GetOrAdd(key, (k) => new AsyncLocal<object?>());
        return asyncLocal.Value;
    }
}
