using System.Globalization;
using I18n.LocalizationExtensions;
using I18n.LocalizationExtensions.Samples;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLocalization();

builder.Services.AddJsonLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var supportedCultures = new List<CultureInfo>
{
    new("en-US"),
    new("en-GB"),
    new("fr"),
    new("ja-JP"),
    new("zh-cn")
};

var options = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("zh-cn"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
};

app.UseRequestLocalization(options);

app.UseAuthorization();

app.MapControllers();

app.MapGet("/demo", async (context) =>
{
    context.Response.ContentType = "text/plain;charset=utf-8";

    IStringLocalizer localizer1 = context.RequestServices.GetRequiredService<IStringLocalizer>();
    await context.Response.WriteAsync($"{localizer1["HI"]}!!");

    IStringLocalizer<InnerSource> localizer2 = context.RequestServices.GetRequiredService<IStringLocalizer<InnerSource>>();
    await context.Response.WriteAsync($"{localizer2["HI"]}!!");
});

app.Run();


namespace I18n.LocalizationExtensions.Samples
{
    public class InnerSource
    {

    }
}
