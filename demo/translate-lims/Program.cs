using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Translate.Data;
using Translate.Services;

namespace Translate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMiniProfiler(options => { options.RouteBasePath = "/profiler"; }).AddEntityFramework();

            var connection = CreateMemoryConnection();
            builder.Services.AddDbContext<TranslateDbContext>(builder =>
            {
                builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                builder.UseSqlite(connection);
                builder.UseSnakeCaseNamingConvention();
                //builder.UseSqlServer(Configuration.GetConnectionString("MSSQL"));
                //builder.UseMySql(Configuration.GetConnectionString("MYSQL"), ServerVersion.AutoDetect(Configuration.GetConnectionString("MYSQL")));
            });

            //builder.Services.AddTransient<LibreTranslate.Net.LibreTranslate>(sp => new LibreTranslate.Net.LibreTranslate("http://vm.qq.com:5000"));

            builder.Services.AddTransient<ICollectService, CollectService>();
            builder.Services.AddTransient<FrontFragmentCollector>();
            builder.Services.AddTransient<BackendFragmentCollector>();
            builder.Services.Configure<CollectorOptions>(options =>
            {
                options.FragmentCollectors.Add(typeof(FrontFragmentCollector));
                options.FragmentCollectors.Add(typeof(BackendFragmentCollector));
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseMiniProfiler();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static SqliteConnection CreateMemoryConnection()
        {
            //var connection = new SqliteConnection("Data Source=:memory:");
            var connection = new SqliteConnection("Data Source = D:\\Code\\dotnet\\Translate\\Translate.db");
            connection.Open();

            using (var context = new TranslateDbContext(new DbContextOptionsBuilder<TranslateDbContext>().UseSqlite(connection).UseSnakeCaseNamingConvention().Options))
            {
                var success = context.GetService<IRelationalDatabaseCreator>().EnsureCreated();
            }

            return connection;
        }
    }
}