using System.Collections.Concurrent;
using StackExchange.Profiling;
using StackExchange.Profiling.Internal;

namespace Yi.Framework.SqlSugarCore.Profilers;

/// <summary>
///  MiniProfiler
/// </summary>
public class SqlSugarDiagnosticListener : IMiniProfilerDiagnosticListener
{
    public string ListenerName => "SQLSugar";

    private readonly ConcurrentDictionary<Guid, CustomTiming> _commands = new();

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(KeyValuePair<string, object> kv)
    {
        if (kv.Key == LogExecutingEvent.EventName)
        {
            if (kv.Value is LogExecutingEvent data)
            {
                MiniProfiler.Current?.CustomTiming(LogExecutingEvent.EventName, data.Text);
            }
        }
    }
}