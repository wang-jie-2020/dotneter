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

            //IP���˿ڵ�
            var ip = GetLocalIp();
            var port = Configuration.GetValue<int>("Port");
            services.Configure<ServiceInfo>(options =>
            {
                options.IPAddress = ip;
                options.Port = port;
            });

            //consul��ַ
            var address = "http://192.168.209.128:18401";
            var datacenter = "dc1";

            //consul����ע��
            services.AddConsulClient("consul", options =>
            {
                options.Address = address;
                options.Datacenter = datacenter;
                //options.Token = "token";//�����token
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
                client.BaseAddress = new Uri("http://server");//server��Server��http����ע���Consul�ķ�����
            }).AddServiceDiscovery("consul", LoadBalancerMode.RoundRobin);//��ӷ����ֻ��ƣ�consulName��AddConsulClient��ӵĿͻ�������

            //grpc client
            services.AddGrpcClient<WebApiServer.WebApiServerClient>("grpc", options =>
            {
                options.Address = new Uri("http://server_grpc");//server_grpc��Server��grpc����ע���Consul�ķ�����
            })
            .AddServiceDiscoveryPolling(options =>//��ӷ����ֻ���
            {
                options.Address = address;
                options.Datacenter = datacenter;
                //options.Token = "token";//�����token
            });
            //����Grpc����Ҫ��TLS֧��
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            //Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "�ͻ��˽ӿ�", Version = "v1" });
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
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "�ͻ��˽ӿ�Api v1");
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
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
