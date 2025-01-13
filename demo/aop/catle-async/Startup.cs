using System;
using System.Net.NetworkInformation;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Demo.Aop;
using Demo.Controllers;
using Demo.Data;
using Demo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
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
            #region 常规部分

            var connection = new SqliteConnection("Data Source =:memory:");
            services.AddSingleton(connection);

            services.AddMvc(options =>
            {

            });

            services.AddDbContext<ApplicationDbContext>(builder =>
            {
                builder.UseSqlite(connection);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            #endregion
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerService>().As<ICustomerService>()
                .InstancePerLifetimeScope()
                .EnableClassInterceptors();

            builder.RegisterType<CustomerManager>().As<ICustomerManager>()
                .InstancePerLifetimeScope()
                .EnableClassInterceptors();

            builder.RegisterType<ReadingAsyncInterceptor>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ReadingAsyncInterceptorAdapter>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>()
                .InstancePerLifetimeScope()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(ReadingAsyncInterceptorAdapter));
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

            using var services = app.ApplicationServices.CreateScope();
            {
                var controller = services.ServiceProvider.GetService<HealthController>();
                Console.WriteLine($"{nameof(HealthController)} is {(controller == null ? "未注册的" : "注册的")}");
            }

            DatabaseInitializer.Seed(services.ServiceProvider);
        }
    }
}
