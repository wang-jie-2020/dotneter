using Demo.Data;
using Demo.EFCoreExtensions;
using Demo.Uow;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using UnitOfWorkContextRepository.Extensions;
using UnitOfWorkContextRepository.Interceptors;

namespace Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connection = CreateMemoryConnection();
            services.AddDbContext<ApplicationDbContext>(builder =>
            {
                builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                builder.UseSqlite(connection);

                //builder.UseSqlServer(Configuration.GetConnectionString("MSSQL"));
                //builder.ReplaceService<IQuerySqlGeneratorFactory, CustomSqlServerQuerySqlGeneratorFactory>();

                //builder.UseMySql(Configuration.GetConnectionString("MYSQL"), ServerVersion.AutoDetect(Configuration.GetConnectionString("MYSQL")));
            });
            services.AddMiniProfiler(options => { options.RouteBasePath = "/profiler"; }).AddEntityFramework();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddUnitOfWorkRepository<ApplicationDbContext>();

            services.AddTransient<UnitOfWorkActionFilter>();
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.AddService<UnitOfWorkActionFilter>();
            });
        }

        private static SqliteConnection CreateMemoryConnection()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            using (var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options))
            {
                context.GetService<IRelationalDatabaseCreator>().EnsureCreated();
            }

            return connection;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiniProfiler();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            using (var services = app.ApplicationServices.CreateScope())
            {
                DatabaseInitializer.Seed(services.ServiceProvider);
            }
        }
    }
}
