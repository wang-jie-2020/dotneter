using Consul;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Consul
{
    public static class ConsulServiceCollectionExtensions
    {
        /// <summary>
        /// 添加一个Consul客户端
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConsulServiceBuilder AddConsulClient(this IServiceCollection services, Action<ConsulClientOptions> configure)
            => services.AddConsulClient(Options.DefaultName, configure);
        /// <summary>
        /// 添加一个Consul客户端
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConsulServiceBuilder AddConsulClient(this IServiceCollection services, string name, Action<ConsulClientOptions> configure)
        {
            services.Configure(name, configure);

            services.TryAddSingleton<IConsulClientFactory, ConsulClientFactory>();

            if (!services.Any(f => f.ImplementationType == typeof(ConsulRegistrationHostedService)))
            {
                services.AddSingleton<IHostedService, ConsulRegistrationHostedService>();
            }

            var builder = new ConsulServiceBuilder(name, services);
            services.AddSingleton<IConsulServiceBuilder>(builder);
            services.AddLogging();
            return builder;
        }

        /// <summary>
        /// 往Consul客户端中添加一个服务
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IConsulServiceBuilder AddService(this IConsulServiceBuilder builder, Action<ConsulServiceOptions> configure)
        {
            ConsulServiceOptions options = new ConsulServiceOptions();
            configure?.Invoke(options);
            return builder.AddService(options);
        }

        /// <summary>
        /// 创建一个Consul客户端
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IConsulClient CreateClient(this IConsulClientFactory consulClientFactory)
            => consulClientFactory.CreateClient(Options.DefaultName);

        /// <summary>
        /// 添加服务发现的MessageHandler
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddServiceDiscovery(this IHttpClientBuilder builder, LoadBalancerMode mode = LoadBalancerMode.Random)
            => builder.AddServiceDiscovery(Options.DefaultName, mode);
        /// <summary>
        /// 添加服务发现的MessageHandler
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="conulName"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddServiceDiscovery(this IHttpClientBuilder builder, string conulName, LoadBalancerMode mode = LoadBalancerMode.Random)
            => builder.AddServiceDiscovery(new ConsulDiscoveryHttpMessageHandlerOptions() { ConsulName = conulName, Mode = mode });
        /// <summary>
        /// 添加服务发现的MessageHandler
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddServiceDiscovery(this IHttpClientBuilder builder, Action<ConsulClientOptions> configure, LoadBalancerMode mode = LoadBalancerMode.Random)
        {
            string name = Guid.NewGuid().ToString();
            builder.Services.AddConsulClient(name, configure);
            return builder.AddServiceDiscovery(name, mode);
        }
        /// <summary>
        /// 添加服务发现的MessageHandler
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddServiceDiscovery(this IHttpClientBuilder builder, ConsulDiscoveryHttpMessageHandlerOptions options)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddLogging();
            builder.Services.TryAddSingleton<IConsulClientFactory, ConsulClientFactory>();
            builder.Services.TryAddSingleton<ConsulServiceDiscoveryProviderAccessor>();

            builder.AddHttpMessageHandler(serviceProvicer =>
            {
                var providerAccessor = serviceProvicer.GetRequiredService<ConsulServiceDiscoveryProviderAccessor>();
                var loggerFactory = serviceProvicer.GetRequiredService<ILoggerFactory>();
                return new ConsulDiscoveryHttpMessageHandler(options, providerAccessor, loggerFactory);
            });
            return builder;
        }
        /// <summary>
        /// 添加服务发现的MessageHandler
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddServiceDiscovery(this IHttpClientBuilder builder, Action<ConsulDiscoveryHttpMessageHandlerOptions> configure)
        {
            ConsulDiscoveryHttpMessageHandlerOptions options = new ConsulDiscoveryHttpMessageHandlerOptions();
            configure?.Invoke(options);
            return builder.AddServiceDiscovery(options);
        }
        /// <summary>
        /// 添加服务发现的MessageHandler
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <param name="interval">循环读取的时间间隔（毫秒）</param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddServiceDiscoveryPolling(this IHttpClientBuilder builder, Action<ConsulClientOptions> configure, int interval = 10000, LoadBalancerMode mode = LoadBalancerMode.Random)
        {
            string name = Guid.NewGuid().ToString();
            builder.Services.AddConsulClient(name, configure);
            return builder.AddServiceDiscoveryPolling(name, interval, mode);
        }
        /// <summary>
        /// 添加服务发现的MessageHandler
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="interval">循环读取的时间间隔（毫秒）</param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddServiceDiscoveryPolling(this IHttpClientBuilder builder, int interval = 10000, LoadBalancerMode mode = LoadBalancerMode.Random)
            => builder.AddServiceDiscoveryPolling(Options.DefaultName, interval, mode);
        /// <summary>
        /// 添加服务发现的MessageHandler
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="conulName"></param>
        /// <param name="interval">循环读取的时间间隔（毫秒）</param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static IHttpClientBuilder AddServiceDiscoveryPolling(this IHttpClientBuilder builder, string conulName, int interval = 10000, LoadBalancerMode mode = LoadBalancerMode.Random)
            => builder.AddServiceDiscovery(new ConsulDiscoveryHttpMessageHandlerOptions() { ConsulName = conulName, Polling = true, Interval = interval, Mode = mode });
    }
}
