using System;
using System.Net;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Email;

namespace Demo.Extension
{
    /// <summary>
    /// 2020.12.14 
    /// wangjie:for appsetting use only,
    /// </summary>
    public static class LoggerConfigurationEmailExtensions
    {
        public static LoggerConfiguration CustomEmail(this LoggerSinkConfiguration loggerConfiguration
            , CustomEmailConnectionInfo connectionInfo
            , string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
            , LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose
            , int batchPostingLimit = 100
            , TimeSpan? period = null
            , IFormatProvider formatProvider = null
            , string mailSubject = "Log Email")
        {
            return loggerConfiguration.Email(connectionInfo
                , outputTemplate
                , restrictedToMinimumLevel
                , batchPostingLimit
                , period
                , formatProvider
                , mailSubject);
        }

        public class CustomEmailConnectionInfo : EmailConnectionInfo
        {
            public NetworkCredential CustomNetworkCredentials
            {
                get
                {
                    return (NetworkCredential)NetworkCredentials;
                }
                set
                {
                    NetworkCredentials = value;
                }
            }
        }
    }
}