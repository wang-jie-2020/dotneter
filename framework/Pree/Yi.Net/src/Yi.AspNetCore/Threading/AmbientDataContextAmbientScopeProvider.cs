﻿using System.Collections.Concurrent;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Yi.AspNetCore.Threading;

public class AmbientDataContextAmbientScopeProvider<T> : IAmbientScopeProvider<T>
{
    public ILogger<AmbientDataContextAmbientScopeProvider<T>> Logger { get; set; }

    private static readonly ConcurrentDictionary<string, ScopeItem> ScopeDictionary = new ConcurrentDictionary<string, ScopeItem>();

    private readonly IAmbientDataContext _dataContext;

    public AmbientDataContextAmbientScopeProvider([NotNull] IAmbientDataContext dataContext)
    {
        Check.NotNull(dataContext, nameof(dataContext));

        _dataContext = dataContext;
        Logger = NullLogger<AmbientDataContextAmbientScopeProvider<T>>.Instance;
    }

    public T? GetValue(string contextKey)
    {
        var item = GetCurrentItem(contextKey);
        if (item == null)
        {
            return default;
        }

        return item.Value;
    }

    public IDisposable BeginScope(string contextKey, T value)
    {
        var item = new ScopeItem(value, GetCurrentItem(contextKey));

        if (!ScopeDictionary.TryAdd(item.Id, item))
        {
            throw new Exception("Can not add item! ScopeDictionary.TryAdd returns false!");
        }

        _dataContext.SetData(contextKey, item.Id);

        return new DisposeAction<ValueTuple<ConcurrentDictionary<string, ScopeItem>, ScopeItem, IAmbientDataContext, string>>(static (state) =>
        {
            var (scopeDictionary, item,  dataContext, contextKey) = state;

            scopeDictionary.TryRemove(item.Id, out item);

            if (item == null)
            {
                return;
            }
            
            if (item.Outer == null)
            {
                dataContext.SetData(contextKey, null);
                return;
            }

            dataContext.SetData(contextKey, item.Outer.Id);

        }, (ScopeDictionary, item, _dataContext, contextKey));
    }

    private ScopeItem? GetCurrentItem(string contextKey)
    {
        return _dataContext.GetData(contextKey) is string objKey ? ScopeDictionary.GetOrDefault(objKey) : null;
    }

    private class ScopeItem
    {
        public string Id { get; }

        public ScopeItem? Outer { get; }

        public T Value { get; }

        public ScopeItem(T value, ScopeItem? outer = null)
        {
            Id = Guid.NewGuid().ToString();

            Value = value;
            Outer = outer;
        }
    }
}
