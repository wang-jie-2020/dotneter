using System.Globalization;
using i18n.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // 首字母小写（驼峰样式）
    
    //options.SerializerSettings.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // 忽略循环引用
    
    // options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified;
    
    //options.SerializerSettings.DateParseHandling = DateParseHandling.None;
    //options.SerializerSettings.DateParseHandling = DateParseHandling.DateTime;
    //options.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
    
    //options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // 时间格式化
    options.SerializerSettings.Converters.Add(new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal });
    // options.SerializerSettings.Converters.Add(new LongJsonConverter()); // long转string（防止js精度溢出） 超过16位开启
    // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; // 忽略空值
});
// builder.Services.AddRouting(o =>
// {
//     o.AppendTrailingSlash = true;
//     o.LowercaseUrls = true;
//     o.LowercaseQueryStrings = true;
// });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MySqlContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseMySql(builder.Configuration.GetConnectionString("MYSQL"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MYSQL")));
});

builder.Services.AddDbContext<MsSqlContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL"));
});

builder.Services.AddDbContext<OracleContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseOracle(builder.Configuration.GetConnectionString("ORACLE"));
});

builder.Services.AddMiniProfiler(options => { options.RouteBasePath = "/profiler"; }).AddEntityFramework();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMiniProfiler();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLocalization(options =>
{
    options.SupportedCultures = new List<CultureInfo>
    {
        new("en-US"),
        new("fr-FR"),
        new("zh-CN")
    };
});

app.UseAuthorization();

app.MapControllers();

app.Run();