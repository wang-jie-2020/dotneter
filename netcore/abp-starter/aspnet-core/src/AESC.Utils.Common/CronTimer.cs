using Cronos;

namespace AESC.Utils.Common
{
    /// <summary>
    /// cron : [秒] [分] [时] [日] [月] [周] [年]
    /// </summary>
    public static class CronTimer
    {
        public static DateTime? GetNextOccurrence(string expression)
        {
            CronExpression.TryParse(expression, CronFormat.IncludeSeconds, out CronExpression cronExpression);
            return cronExpression?.GetNextOccurrence(DateTime.UtcNow);
        }

        public static IEnumerable<DateTime?> GetOccurrences(string expression, int n = 5)
        {
            var index = 0;

            while (index < n)
            {
                index++;
                yield return GetNextOccurrence(expression);
            }
        }
    }
}
