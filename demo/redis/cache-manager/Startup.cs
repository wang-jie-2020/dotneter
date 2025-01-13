using System;
using CacheManager.Core;
using CacheManager.Core.Logging;
using Demo.Data;
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

            services.AddMvc();

            services.AddDbContext<ApplicationDbContext>(builder =>
            {
                builder.UseSqlite(connection);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
            /*
            /*	 									CacheManager
            /*	
            /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

            var builder = new CacheManager.Core.ConfigurationBuilder();
            builder.WithJsonSerializer();
            builder.WithMaxRetries(10);

            //redis
            builder.WithRedisConfiguration("local-redis-cache", options =>
            {
                options.WithAllowAdmin().WithDatabase(0).WithEndpoint("localhost", 6379);
            })
            .WithRedisConfiguration("remote-redis-cache", options =>
            {
                options.WithAllowAdmin().WithDatabase(0).WithEndpoint("vm.qq.com", 6379);
            });

            builder.WithRedisCacheHandle("local-redis-cache", false).WithExpiration(ExpirationMode.Sliding, TimeSpan.FromSeconds(600));
            builder.WithRedisCacheHandle("remote-redis-cache", true).WithExpiration(ExpirationMode.Sliding, TimeSpan.FromSeconds(600));

            //memory
            //builder.WithMicrosoftMemoryCacheHandle().WithExpiration(ExpirationMode.Sliding, TimeSpan.FromSeconds(600));

            //services.AddCacheManagerConfiguration(Configuration);
            //services.AddCacheManager<int>(Configuration, configure: builder => builder.WithJsonSerializer());
            //services.AddCacheManager<DateTime>(inline => inline.WithDictionaryHandle());

            services.AddSingleton(builder.Build());
            services.AddCacheManager();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
