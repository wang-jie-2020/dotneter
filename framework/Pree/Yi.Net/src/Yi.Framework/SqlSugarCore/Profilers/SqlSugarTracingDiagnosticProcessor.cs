using System.Collections.Concurrent;
using SkyApm;
using SkyApm.Common;
using SkyApm.Config;
using SkyApm.Diagnostics;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;

namespace Yi.Framework.SqlSugarCore.Profilers;

/// <summary>
///  SkyWalking
/// </summary>
public class SqlSugarTracingDiagnosticProcessor : ITracingDiagnosticProcessor
{
    public string ListenerName => "SQLSugar";

    private readonly ConcurrentDictionary<Guid, SegmentContext> _contexts = new();

    private readonly ITracingContext _tracingContext;
    private readonly IEntrySegmentContextAccessor _entrySegmentContextAccessor;
    private readonly IExitSegmentContextAccessor _exitSegmentContextAccessor;
    private readonly ILocalSegmentContextAccessor _localSegmentContextAccessor;
    private readonly TracingConfig _tracingConfig;

    public SqlSugarTracingDiagnosticProcessor(ITracingContext tracingContext,
        IEntrySegmentContextAccessor entrySegmentContextAccessor,
        IExitSegmentContextAccessor exitSegmentContextAccessor,
        ILocalSegmentContextAccessor localSegmentContextAccessor,
        IConfigAccessor configAccessor)
    {
        _tracingContext = tracingContext;
        _exitSegmentContextAccessor = exitSegmentContextAccessor;
        _localSegmentContextAccessor = localSegmentContextAccessor;
        _entrySegmentContextAccessor = entrySegmentContextAccessor;
        _tracingConfig = configAccessor.Get<TracingConfig>();
    }

    [DiagnosticName(LogExecutingEvent.EventName)]
    public void Logging([Object] LogExecutingEvent eventData)
    {
        var context = _tracingContext.CreateExitSegmentContext("database", eventData.Url);
        context.Span.SpanLayer = SpanLayer.DB;
        context.Span.Component = Components.SQLCLIENT;
        context.Span.AddTag(Tags.DB_TYPE, "sql");
        context.Span.AddTag(Tags.DB_INSTANCE, eventData.Url);
        context.Span.AddTag(Tags.DB_STATEMENT, eventData.Text);
        _tracingContext.Release(context);
    }
}