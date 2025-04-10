using Nacos.V2;
using Nacos.V2.DependencyInjection;
using NacosConfiguration;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseNacosConfig("Nacos");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Email>(builder.Configuration.GetSection("Email"));

// builder.Services.AddNacosV2Config(builder.Configuration);  // 注意这个近似方法作用是暴露INacosConfigService而不是ConfigurationSource
builder.Configuration.AddNacosV2Configuration(builder.Configuration.GetSection("Nacos"));
// builder.Configuration.AddNacosV2Configuration(nacos =>
// {
//     nacos.ServerAddresses = ["127.0.0.1"];
//     nacos.Namespace = "nacos";
//     nacos.Listeners =
//     [
//         new() { DataId = "", Group = "", Optional = true },
//         new() { DataId = "", Group = "", Optional = true },
//         new() { DataId = "", Group = "", Optional = true }
//     ];
// });

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();

app.Run();