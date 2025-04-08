using Serilog.Events;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Newtonsoft.Json.Converters;
using Volo.Abp.Modularity;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Error)
    .MinimumLevel.Override("Quartz", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.File("logs/debug-.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Debug))
    .WriteTo.Async(c => c.File("logs/info-.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Information))
    .WriteTo.Async(c => c.File("logs/error-.log", rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Error))
    .WriteTo.Async(c => c.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}"
        , theme: AnsiConsoleTheme.Code))
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication<MainModule>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
await app.InitializeAsync();

app.Run();

internal class MainModule : AbpModule
{
    
}