using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AspNetCore.Kafka;
using AspNetCore.Kafka.Integration;
using AspNetCore.Kafka.Logger;

namespace AspNetCore.WebApi.Consumer
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
            var hosts = new string[] { "192.168.209.133:9092", "192.168.209.134:9092", "192.168.209.135:9092" };

            #region ��־��¼

            services.AddKafkaConsumer(options =>
            {
                options.BootstrapServers = hosts;
                options.EnableAutoCommit = true;//�Զ��ύ
                options.GroupId = "group.1";
                options.Subscribers = KafkaSubscriber.From("topic.logger");

            }).AddListener(result =>
            {
                Console.WriteLine("Message From topic.logger:" + result.Message);
            });

            #endregion

            #region Kafka

            services.AddKafkaConsumer(options =>
            {
                options.BootstrapServers = hosts;
                options.EnableAutoCommit = false;
                options.GroupId = "group.2";
                options.Subscribers = KafkaSubscriber.From("topic.kafka");

            }).AddListener(result =>//ֱ����lambda���ʽ����������߼�
            {
                Console.WriteLine("Message From topic.kafka:" + result.Message);
                result.Commit();
            }).AddListener<KafkaConsumerListener>();//ʵ��IKafkaConsumerListener�ӿ���������߼�

            #endregion

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
