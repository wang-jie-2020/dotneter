using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Api.Entry;

public class Program
{
    public static void Main(string[] args)
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
        
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseAutofac();
        builder.Host.UseSerilog();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddApplication<Entry>(); 

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapControllers();
        app.InitializeAsync();
        
        app.Run();
    }
}