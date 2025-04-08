using Consul;
using Custom.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;

namespace Custom
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("custom", new OpenApiInfo { Title = "Custom API", Version = "v1" });
            });
            services.Configure<ConsulConfig>(Configuration.GetSection("consul"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "swagger";
                c.SwaggerEndpoint("/swagger/custom/swagger.json", "Custom API V1");
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            var consulConfig = app.ApplicationServices.GetRequiredService<IOptions<ConsulConfig>>().Value;
            foreach (var service in consulConfig.Services)
            {
                var consulClient = new ConsulClient(x =>
                {
                    x.Address = new Uri($"http://{consulConfig.Server.Ip}:{consulConfig.Server.Port}");
                });

                var httpCheck = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    Interval = TimeSpan.FromSeconds(10),
                    HTTP = $"http://{service.Ip}:{service.Port}/api/health",
                    Timeout = TimeSpan.FromSeconds(5)
                };

                var registration = new AgentServiceRegistration()
                {
                    Checks = new[] { httpCheck },
                    ID = Guid.NewGuid().ToString(),
                    Name = service.Name,
                    Address = service.Ip,
                    Port = service.Port,
                    Tags = new[] { $"urlprefix-/{service.Name}" }
                };

                consulClient.Agent.ServiceRegister(registration).Wait();
                lifetime.ApplicationStopping.Register(() =>
                {
                    //服务停止时取消注册
                    consulClient.Agent.ServiceDeregister(registration.ID).Wait();
                });
            }
        }
    }
}
