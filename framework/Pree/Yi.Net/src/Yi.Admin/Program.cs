using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Yi.AspNetCore.Extensions.Builder;
using Yi.Web;

Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Error)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("logs/yi/debug-.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Debug))
    .WriteTo.Async(c => c.File("logs/yi/info-.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information))
    .WriteTo.Async(c => c.File("logs/yi/error-.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error))
    .WriteTo.Async(c => c.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}"
        , theme: AnsiConsoleTheme.Code))
    .CreateLogger();

try
{
    Log.Information("""

        __     ___   ______                                           _    
        \ \   / (_) |  ____|                                         | |   
         \ \_/ / _  | |__ _ __ __ _ _ __ ___   _____      _____  _ __| | __
          \   / | | |  __| '__/ _` | '_ ` _ \ / _ \ \ /\ / / _ \| '__| |/ /
           | |  | | | |  | | | (_| | | | | | |  __/\ V  V / (_) | |  |   < 
           |_|  |_| |_|  |_|  \__,_|_| |_| |_|\___| \_/\_/ \___/|_|  |_|\_\
   
     """);
    
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseAutofac();
    builder.Host.UseSerilog();
    await builder.Services.AddApplicationAsync<AdminModule>();
    var app = builder.Build();
    await app.InitializeApplicationAsync();
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex.Message);
}
finally
{
    Log.CloseAndFlush();
}