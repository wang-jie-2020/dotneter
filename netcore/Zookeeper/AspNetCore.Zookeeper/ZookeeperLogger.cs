using System;
using System.Diagnostics;
using org.apache.utils;

namespace AspNetCore.Zookeeper
{
    /// <summary>
    /// Zookeeper默认日志记录
    /// </summary>
    public class ZookeeperLogger : ILogConsumer
    {
        /// <summary>
        /// 是否记录日志到文件
        /// </summary>
        public bool LogToFile { get; set; } = false;

        /// <summary>
        /// 是否记录堆栈信息
        /// </summary>
        public bool LogToTrace { get; set; } = true;

        /// <summary>
        /// 日志级别
        /// </summary>
        public TraceLevel LogLevel { get; set; } = TraceLevel.Warning;

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="severity"></param>
        /// <param name="className"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public virtual void Log(TraceLevel severity, string className, string message, Exception exception)
        {
            Console.WriteLine(string.Format("Level:{0}  className:{1}   message:{2}", severity, className, message));
            Console.WriteLine(exception?.StackTrace);
        }
    }
}