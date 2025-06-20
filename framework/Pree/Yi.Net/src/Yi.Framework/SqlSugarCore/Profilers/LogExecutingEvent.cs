namespace Yi.Framework.SqlSugarCore.Profilers;

public class LogExecutingEvent
{
    public const string EventName = "SQLSugar.LogExecuting";

    public Guid TraceId { get; }

    public string Text { get; }
    
    public string Url { get; }

    public LogExecutingEvent(Guid traceId, string url, string text)
    {
        TraceId = traceId;
        Url = url;
        Text = text;
    }
}