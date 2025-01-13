using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace Demo.Extension
{
    /// <summary>
    /// 2020.12.14 
    /// wangjie:for appsetting use only,not so good
    /// </summary>
    public static class LoggerConfigurationMySqlExtensions
    {
        public static LoggerConfiguration CustomMySQL(this LoggerSinkConfiguration loggerConfiguration
            , string connectionStringName
            , string tableName = "Logs"
            , LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose
            , bool storeTimestampInUtc = false
            , uint batchSize = 100u
            , LoggingLevelSwitch levelSwitch = null)
        {
            var connectionString = Global.Configuration?.GetSection("ConnectionStrings")?[connectionStringName];

            return loggerConfiguration.MySQL(connectionString
                , tableName
                , restrictedToMinimumLevel
                , storeTimestampInUtc
                , batchSize
                , levelSwitch);
        }
    }
}
