using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Runtime.InteropServices;

namespace MAT;


public class Program
{
    public static async Task Main(string[] args)
    {
        Environment.SetEnvironmentVariable("PATH",
        @"C:\Program Files\MATLAB\MATLAB Runtime\R2024a\runtime\win64;" +
            Environment.GetEnvironmentVariable("PATH"),
        EnvironmentVariableTarget.Process);


        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Error)
            .MinimumLevel.Override("Volo.Abp", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("logs/debug-.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Debug))
            .WriteTo.Async(c => c.File("logs/info-.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information))
            .WriteTo.Async(c => c.File("logs/error-.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error))
            .WriteTo.Async(c => c.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}"
                , theme: AnsiConsoleTheme.Code))
            .CreateLogger();

        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddSerilog();

        builder.Services.AddHostedService<FailureCaller>();

        var host = builder.Build();
        await host.RunAsync();
    }
}
