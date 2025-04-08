using AspNetCore.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Grpc;

namespace AspNetCore.WebApi.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TestOptions>(Configuration);

            //IP及端口等
            var ip = GetLocalIp();
            var port = Configuration.GetValue<int>("Port");
            services.Configure<ServiceInfo>(options =>
            {
                options.IPAddress = ip;
                options.Port = port;
            });

            //consul地址
            var address = "http://192.168.209.128:18401";
            var datacenter = "dc1";

            //consul服务注册
            services.AddConsulClient("consul", options =>
            {
                options.Address = address;
                options.Datacenter = datacenter;
                //options.Token = "token";//如果有token
            }).AddService(options =>
            {
                options.Host = ip;
                options.Port = port;
                options.Id = $"client_{ip}_{port}";
                options.Name = "client";
                options.Tags = new[] { "client" };
                options.HealthCheckPath = "Health";
            });

            //http client
            services.AddHttpClient("http", client =>
            {
                client.BaseAddress = new Uri("http://server");//server是Server的http服务注册进Consul的服务名
            }).AddServiceDiscovery("consul", LoadBalancerMode.RoundRobin);//添加服务发现机制，consulName是AddConsulClient添加的客户端名称

            //grpc client
            services.AddGrpcClient<WebApiServer.WebApiServerClient>("grpc", options =>
            {
                options.Address = new Uri("http://server_grpc");//server_grpc是Server的grpc服务注册进Consul的服务名
            })
            .AddServiceDiscoveryPolling(options =>//添加服务发现机制
            {
                options.Address = address;
                options.Datacenter = datacenter;
                //options.Token = "token";//如果有token
            });
            //设置Grpc不需要用TLS支持
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            //Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "客户端接口", Version = "v1" });
                options.IncludeXmlComments($"{typeof(Startup).Assembly.GetName().Name}.xml", true);
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "客户端接口Api v1");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        /// <summary>
        /// 获取本地Ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            var addressList = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList;
            var ips = addressList.Where(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(address => address.ToString()).ToArray();
            if (ips.Length == 1)
            {
                return ips.First();
            }
            return ips.Where(address => !address.EndsWith(".1")).FirstOrDefault() ?? ips.FirstOrDefault();
        }
    }
}
