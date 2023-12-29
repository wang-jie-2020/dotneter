using AspNetCore.Consul;
using AspNetCore.Consul.Configurations;
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

namespace AspNetCore.WebApi.Server
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

            //IP���˿ڵ�
            var ip = GetLocalIp();
            var port = Configuration.GetValue<int>("Port");
            var grpcPort = Configuration.GetValue<int>("GrpcPort");
            services.Configure<ServiceInfo>(options =>
            {
                options.IPAddress = ip;
                options.Port = port;
                options.GrpcPort = grpcPort;
            });

            //consul��ַ
            var address = "http://192.168.209.128:18401";
            var datacenter = "dc1";

            //consul����ע��
            services.AddConsulClient(options =>
            {
                options.Address = address;
                options.Datacenter = datacenter;
                //options.Token = "token";//�����token
            }).AddService(options =>
            {
                //http����
                options.Host = ip;
                options.Port = port;
                options.Id = $"server_{ip}_{port}";
                options.Name = "server";
                options.Tags = new[] { "server" };
                options.HealthCheckPath = "Health";
            }).AddService(options =>
            {
                //grpc����
                options.Host = ip;
                options.Port = grpcPort;
                options.Id = $"server_grpc_{ip}_{grpcPort}";
                options.Name = "server_grpc";
                options.Tags = new[] { "server" };
                //options.HealthCheckUrl = $"http://{ip}:{port}/Health";//ʹ��http�Ľ������

                //ʹ��grpc���������
                options.HealthCheckUseGrpc = true;
                options.HealthCheckUrl = $"{ip}:{grpcPort}";//����health.proto
            });

            //Grpc��������
            services.AddGrpc();

            //Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "����˽ӿ�", Version = "v1" });
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

            //����http://host:port/consul?name=reloadName�������¼���IConfiguration
            app.UseConsulWatch();
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "����˽ӿ�Api v1");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<WebApiImpl>();//Grpc
                endpoints.MapGrpcService<HealthImpl>();//health check
                endpoints.MapControllers();
            });
        }
        /// <summary>
        /// ��ȡ����Ip��ַ
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
