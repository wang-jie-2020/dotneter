namespace Yi.AspNetCore.SqlSugarCore.Profilers;

public class LogExecutingEvent
{
    public const string EventName = "SQLSugar.LogExecuting";
    
    public Guid TraceId { get; }

    public string Text { get; }

    public LogExecutingEvent(Guid traceId, string text)
    {
        TraceId = traceId;
        Text = text;
    }
}