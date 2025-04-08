using AspNetCore.Activemq.Integration;
using AspNetCore.Activemq.Logger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AspNetCore.Activemq.Test
{
    public class ActiveMqServiceTest : BaseUnitTest
    {
        ITestOutputHelper Output;
        public ActiveMqServiceTest(ITestOutputHelper testOutputHelper)
        {
            Output = testOutputHelper;
        }

        private void WriteLogger(RecieveResult result)
        {
            var destination = result.Destination;
            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(result.Message);
            var applicationName = dict[nameof(ActiveLoggerMessage<string>.ApplicationName)];
            var category = dict[nameof(ActiveLoggerMessage<string>.Category)];
            var logLevel = dict[nameof(ActiveLoggerMessage<string>.LogLevel)];
            var message = dict[nameof(ActiveLoggerMessage<string>.Message)];
            Output.WriteLine($@"��{destination}��{applicationName}:{category}-{logLevel}-{message}");
        }
        /// <summary>
        /// ��־����
        /// </summary>
        [Fact]
        public async Task LoggerTest()
        {
            int counter = 0;
            ActiveServer activeServer = new ActiveServer();
            activeServer.Register(services =>
            {
                services.AddLogging(builder =>
                {
                    builder.SetMinimumLevel(LogLevel.Trace);
                });
                services.AddActiveLogger(options =>
                {
                    options.IsCluster = true;
                    options.Protocol = protocol;
                    options.ApplicationName = nameof(ActiveMqServiceTest);
                    options.BrokerUris = brokerUris;
                    options.Category = nameof(ActiveMqServiceTest);
                    options.UseQueue = true;
                    options.Destination = $"service.{logger}";
                    options.MinLevel = LogLevel.Trace;
                    options.InitializeCount = 10;
                    options.IsPersistent = true;
                    options.Password = password;
                    options.UserName = userName;
                });

                services.AddActiveConsumer(options =>
                {
                    options.IsCluster = true;
                    options.Protocol = protocol;
                    options.BrokerUris = brokerUris;
                    options.Durable = false;
                    options.FromQueue = true;
                    options.Destination = $"service.{logger}";
                    options.AutoAcknowledge = true;
                    options.Password = password;
                    options.UserName = userName;
                }).AddListener(result =>
                {
                    WriteLogger(result);
                    if (result.Message.Contains(nameof(ActiveMqServiceTest)))
                    {
                        counter++;
                    }
                });
            });
            await activeServer.StartAsync();

            Thread.Sleep(1000);//�ȴ�����1��

            var serviceProvider = activeServer.ServiceProvider;
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            var _logger = loggerFactory.CreateLogger<ActiveMqServiceTest>();

            _logger.LogTrace("LogTrace");
            _logger.LogDebug("LogDebug");
            _logger.LogInformation("LogInformation");
            _logger.LogWarning("LogWarning");
            _logger.LogError("LogError");
            _logger.LogCritical("LogCritical");

            BlockUntil(() => counter >= 6, 3000);

            Thread.Sleep(1000);//�ȴ�����1��
            Assert.Equal(6, counter);

            await activeServer.StopAsync();
        }
        /// <summary>
        /// ���м��ɲ���
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task QueueTest()
        {
            int counter = 0;
            ActiveServer activeServer = new ActiveServer();
            activeServer.Register(services =>
            {
                services.AddActiveProducer(options =>
                {
                    options.IsCluster = true;
                    options.Protocol = protocol;
                    options.BrokerUris = brokerUris;
                    options.Destination = $"service.{queue}";
                    options.IsPersistent = true;
                    options.Transactional = false;
                    options.Password = password;
                    options.UserName = userName;
                });

                services.AddActiveConsumer(options =>
                {
                    options.IsCluster = true;
                    options.Protocol = protocol;
                    options.BrokerUris = brokerUris;
                    options.Durable = false;
                    options.Destination = $"service.{queue}";
                    options.AutoAcknowledge = false;
                    options.FromQueue = true;
                    options.Password = password;
                    options.UserName = userName;
                }).AddListener(result =>
                {
                    Output.WriteLine(JsonSerializer.Serialize(result));
                    counter++;
                    result.Commit();
                });
            });
            await activeServer.StartAsync();

            Thread.Sleep(1000);//�ȴ�����1��

            var serviceProvider = activeServer.ServiceProvider;
            var factory = serviceProvider.GetService<IActiveProducerFactory>();
            var producer = factory.Create();

            for (var i = 0; i < 10; i++)
            {
                await producer.SendAsync("message" + i);
            }

            BlockUntil(() => counter >= 10, 3000);

            Thread.Sleep(1000);//�ȴ�����1��
            Assert.Equal(10, counter);

            await activeServer.StopAsync();
        }
        /// <summary>
        /// Topic���ɲ���
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TopicTest()
        {
            int counter = 0;
            ActiveServer activeServer = new ActiveServer();
            activeServer.Register(services =>
            {
                services.AddActiveProducer(options =>
                {
                    options.IsCluster = true;
                    options.Protocol = protocol;
                    options.BrokerUris = brokerUris;
                    options.Destination = $"service.{topic}";
                    options.IsPersistent = true;
                    options.Transactional = false;
                    options.Password = password;
                    options.UserName = userName;
                });

                services.AddActiveConsumer(options =>
                {
                    options.IsCluster = true;
                    options.Protocol = protocol;
                    options.BrokerUris = brokerUris;
                    options.Durable = false;
                    options.Destination = $"service.{topic}";
                    options.AutoAcknowledge = false;
                    options.FromQueue = false;
                    options.Password = password;
                    options.UserName = userName;
                    options.ClientId = "service.topic";
                    options.PrefetchCount = 10;
                }).AddListener(result =>
                {
                    Output.WriteLine(JsonSerializer.Serialize(result));
                    counter++;
                    result.Commit();
                });
            });
            await activeServer.StartAsync();

            Thread.Sleep(1000);//�ȴ�����1��

            var serviceProvider = activeServer.ServiceProvider;
            var factory = serviceProvider.GetService<IActiveProducerFactory>();
            var producer = factory.Create();

            for (var i = 0; i < 10; i++)
            {
                await producer.PublishAsync("message" + i);
            }

            BlockUntil(() => counter >= 10, 3000);

            Thread.Sleep(1000);//�ȴ�����1��
            Assert.Equal(10, counter);

            await activeServer.StopAsync();
        }
    }
}
