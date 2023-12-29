using System;
using System.Threading.Tasks;
using Demo.EntityFrameworkCore;
using Demo.EntityFrameworkCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvc();

            var connectionString = Configuration.GetConnectionString("Default");

            //services.Configure<ConfigureDbContextOptions>(options =>
            //{
            //    options.Configure(builder =>
            //    {
            //        builder.DbContextOptionsBuilder.
            //            UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            //    });
            //});

            //services.Configure<ConfigureDbContextOptions>(options =>
            //{
            //    options.UseMySQL(connectionString);
            //});

            services.Configure<ConfigureDbContextOptions>(options =>
            {
                options.UseMySQL();
            });

            services.AddDbContext<UserDbContext>();
            services.AddTransient(ConfigureDbContextOptionsFactory.CreateContextOptions<UserDbContext>);

            services.AddDbContext<OrderDbContext>();
            services.AddTransient(ConfigureDbContextOptionsFactory.CreateContextOptions<OrderDbContext>);

            services.AddDbContext<ProductDbContext>();
            services.AddTransient(ConfigureDbContextOptionsFactory.CreateContextOptions<ProductDbContext>);

            services.AddTransient<IConnectionStringResolver, DefaultConnectionResolver>();

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger().UseSwaggerUI();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Task.Run(async () =>
            {
                using (var services = app.ApplicationServices.CreateScope())
                {
                    await DatabaseInitializer.SeedAsync(services.ServiceProvider);
                }
            });
        }
    }
}
