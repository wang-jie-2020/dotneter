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

            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlite(connection));

            services.AddScoped<IAlbumService, AlbumEfService>();

            services.AddMiniProfiler(options =>
            {
                // �趨�������ڵ�λ�������½�
                //options.PopupRenderPosition = RenderPosition.BottomLeft;

                // �趨�ڵ�������ϸ���������ʽTime With Children����
                //options.PopupShowTimeWithChildren = true;

                // �趨���ʷ������URL��·�ɻ���ַ
                options.RouteBasePath = "/profiler";

            }).AddEntityFramework();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiniProfiler();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var services = app.ApplicationServices.CreateScope())
            {
                DatabaseInitializer.Seed(services.ServiceProvider);
            }
        }
    }
}
