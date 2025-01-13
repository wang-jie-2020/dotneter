using System;
using AspectCore.Configuration;
using AspectCore.Extensions.Autofac;
using AspectCore.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Demo.Aop;
using Demo.Controllers;
using Demo.Data;
using Demo.Services;
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
            #region 非aop内容

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

            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ICustomerManager, CustomerManager>();

            //services.AddTransient<ServiceInterceptorAttribute>(); ServiceInterceptorAttribute 注释掉了,名称和组件里的类似，容易混淆

            services.AddTransient<ManagerInterceptorAttribute>();
            services.AddTransient<CustomerInterceptorAttribute>();

            //services.ConfigureDynamicProxy(configure =>
            //{
            //    configure.Interceptors.AddServiced<ManagerInterceptorAttribute>(Predicates.ForService("*Manager"));
            //});
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //注意作者标注：调用Populate扩展方法在Autofac中注册已经注册到ServiceContainer中的服务（如果有）。注：此方法调用应在RegisterDynamicProxy之前
            //简单说：必须由autofac接管后才能正常的添加代理设置
            builder.RegisterDynamicProxy(configure =>
            {
                //注册动态代理的配置语法不详细看了
                configure.Interceptors.AddServiced<ManagerInterceptorAttribute>(Predicates.ForService("*Manager"));
            });
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
                //controller == null !!!
                var controller = services.ServiceProvider.GetService<HealthController>();
                Console.WriteLine($"{nameof(HealthController)} is {(controller == null ? "未注册的" : "注册的")}");
            }

            DatabaseInitializer.Seed(services.ServiceProvider);
        }
    }
}
