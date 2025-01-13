using System.Reflection;
using Demo.Data;
using Demo.Services;
using Demo.Services.impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
            var connection = new SqliteConnection("Data Source =:memory:");
            services.AddSingleton(connection);

            services.AddControllers();

            services.AddDbContext<ApplicationDbContext>(builder =>
            {
                builder.UseSqlite(connection);
            });

            //Swagger配置
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddMvc();  //AddMiniProfiler似乎是基于MVC的。这一句不加api项目会错误
            services.AddMiniProfiler(options =>
                options.RouteBasePath = "/profiler"
            ).AddEntityFramework();

            services.AddScoped<IAlbumService, AlbumEfService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Swagger配置
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";	//根处提供Swagger UI
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("MiniProfiler-Api.SwaggerIndex.html");
            });

            app.UseMiniProfiler();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var services = app.ApplicationServices.CreateScope())
            {
                DatabaseInitializer.Seed(services.ServiceProvider);
            }
        }
    }
}
