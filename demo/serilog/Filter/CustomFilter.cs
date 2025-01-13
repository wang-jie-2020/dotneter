using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;

namespace Demo.Filter
{
    public class CustomFilter : ILogEventFilter
    {
        public bool IsEnabled(LogEvent logEvent)
        {
            return Matching.FromSource<CustomFilter>()(logEvent);
        }
    }
}
