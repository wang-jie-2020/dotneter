using System.Globalization;
using I18n.LocalizationExtensions.Database;
using I18n.LocalizationExtensions.Database.Context;
using I18n.LocalizationExtensions.Database.I18n;
using Microsoft.AspNetCore.Localization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;

namespace I18n.LocalizationExtensions.Database
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DATABASE 模式
            var connection = CreateMemoryConnection();
            builder.Services.AddDbContext<ResourceDbContext>(builder =>
            {
                builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                builder.UseSqlite(connection);
            });
            
            builder.Services.TryAddSingleton<IStringLocalizerFactory, DatabaseStringLocalizerFactory>();
            builder.Services.TryAddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
            builder.Services.TryAddTransient(typeof(IStringLocalizer), typeof(StringLocalizer));
            builder.Services.TryAddTransient(typeof(IDatabaseStringProvider), typeof(DatabaseStringProvider));

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

            app.MapGet("/culture", async (context) =>
            {
                context.Response.ContentType = "text/plain;charset=utf-8";
                await context.Response.WriteAsync($"{CultureInfo.CurrentCulture.Name}");
            });

            app.MapGet("/demo", async (context) =>
            {
                context.Response.ContentType = "text/plain;charset=utf-8";

                IStringLocalizer localizer1 = context.RequestServices.GetRequiredService<IStringLocalizer>();
                await context.Response.WriteAsync($"{localizer1["HI"]}");
            });

            using (var scope = app.Services.CreateScope())
            {
                using (var context = new ResourceDbContext(new DbContextOptionsBuilder<ResourceDbContext>().UseSqlite(connection).Options))
                {
                    context.GetService<IRelationalDatabaseCreator>().EnsureCreated();

                    context.Resources.Add(new Resource()
                    {
                        Id = 1,
                        Name = "HI",
                        Value = "Hello",
                        Culture = "en-US"
                    });
                    
                    context.Resources.Add(new Resource()
                    {
                        Id = 2,
                        Name = "HI",
                        Value = "bonjour",
                        Culture = "fr"
                    });
                    
                    context.Resources.Add(new Resource()
                    {
                        Id = 3,
                        Name = "HI",
                        Value = "你好",
                        Culture = "zh-CN"
                    });

                    context.SaveChanges();
                }
            }

            app.Run();
        }

        private static SqliteConnection CreateMemoryConnection()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();
            
            return connection;
        }
    }
}