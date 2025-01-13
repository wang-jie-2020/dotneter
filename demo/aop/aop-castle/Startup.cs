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
            builder.ComponentRegistryBuilder.Registered += (sender, e) =>
            {
                var registrationServices = e.ComponentRegistration.Services;

                e.ComponentRegistration.PipelineBuilding += (o, pipelineBuilder) =>
                {

                };
            };

            /*
             *  aop-autofac-castle 的实现中,直接依赖于autofac组件提供的api可以做
             *      事实上如果不侵入autofac,实现其他扩展似乎不太可能，除非类似aspect在解析处理管道中再做代理
             */
            builder.RegisterType<ServiceInterceptor>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CustomerService>().As<ICustomerService>()
                .InstancePerLifetimeScope()
                .EnableClassInterceptors();

            builder.RegisterType<ManagerInterceptor>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CustomerManager>().As<ICustomerManager>()
                .InstancePerLifetimeScope()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(ManagerInterceptor));

            //单个的async-interceptor
            //builder.RegisterType<LoggingAsyncInterceptor>().AsSelf().InstancePerLifetimeScope();
            //builder.RegisterType<LoggingAsyncInterceptorAdapter>().AsSelf().InstancePerLifetimeScope();
            //builder.RegisterType<CustomerRepository>().As<ICustomerRepository>()
            //    .InstancePerLifetimeScope()
            //    .EnableClassInterceptors()
            //    .InterceptedBy(typeof(LoggingAsyncInterceptorAdapter));

            //泛型的adapter
            builder.RegisterGeneric(typeof(GenericAsyncInterceptorAdapter<>)).InstancePerLifetimeScope();
            builder.RegisterType<LoggingAsyncInterceptor>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ReadingAsyncInterceptor>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>()
                .InstancePerLifetimeScope()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(GenericAsyncInterceptorAdapter<>).MakeGenericType(typeof(LoggingAsyncInterceptor)));
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
