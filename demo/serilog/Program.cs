using System;
using System.IO;
using Demo.Controllers;
using Demo.Enrich;
using Demo.Filter;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Demo
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        public static void Main(string[] args)
        {
            Global.Configuration = Configuration;

            Serilog.Debugging.SelfLog.Enable(Console.Error);    //Debug Serilog

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("Version", "1.0.0")
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()

#if DEBUG
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Information)

                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}"
                , theme: AnsiConsoleTheme.Code)

            //.WriteTo.File(path: "logs\\log-.txt"
            //, rollingInterval: RollingInterval.Day
            //, restrictedToMinimumLevel: LogEventLevel.Information)

            //.WriteTo.Email(connectionInfo: new EmailConnectionInfo()
            //{
            //    EmailSubject = "System Error",
            //    FromEmail = "3255401317@qq.com",
            //    MailServer = "smtp.qq.com",
            //    NetworkCredentials = new NetworkCredential(userName: "3255401317@qq.com", password: "xlzyzgqeoyuscjba"),
            //    Port = 587,
            //    ToEmail = "421004034@qq.com,wangjie@wxlgzh.com"
            //}
            //, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}]{NewLine}{Message}{NewLine}{Exception}"
            //, restrictedToMinimumLevel: LogEventLevel.Error)

            .WriteTo.Logger(l =>
            {
                l.Filter.With<CustomFilter>();
                l.Enrich.FromLogContext();
                l.Enrich.With<CustomerEnricher<Operation>>();
                l.WriteTo.File(outputTemplate: "{OrderNo}--{Operation_OperationName}--{Message:lj}{NewLine}"
                    , path: "logs\\user-.txt"
                    , rollingInterval: RollingInterval.Day
                    , restrictedToMinimumLevel: LogEventLevel.Information);
                
                // l.Filter.ByIncludingOnly(Matching.FromSource<UserController>());
                // l.WriteTo.MySQL(connectionString: Configuration.GetConnectionString("Default")
                //                 , tableName: "logs_User"
                //                 , restrictedToMinimumLevel: LogEventLevel.Information);
            })

#else
            .ReadFrom.Configuration(Configuration)
#endif

            .CreateLogger();

            try
            {
                Log.Information("Starting host...");
                CreateHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog();
    }
}



