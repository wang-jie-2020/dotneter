using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Volo.Abp;

namespace Worker.Entry;

public class Program
{
    public static async Task Main(string[] args)
    {
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
        builder.ConfigureContainer(builder.Services.AddAutofacServiceProviderFactory());
        builder.Services.AddSerilog();
        await builder.Services.AddApplicationAsync<Entry>();

        var host = builder.Build();
        await host.InitializeAsync();
        await host.RunAsync();
    }
}