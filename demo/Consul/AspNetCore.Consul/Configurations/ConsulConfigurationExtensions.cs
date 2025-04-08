using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Consul.Configurations
{
    public static class ConsulConfigurationExtensions
    {
        /// <summary>
        /// 添加Consul配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder, Action<ConsulConfigurationOptions> configure)
        {
            var options = new ConsulConfigurationOptions();
            configure?.Invoke(options);
            return builder.AddConsul(options);
        }
        /// <summary>
        /// 添加Consul配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="consulConfigurationOptions"></param>
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder builder, ConsulConfigurationOptions consulConfigurationOptions)
        {
            ConsulConfigurationSource consulConfigurationSource = new ConsulConfigurationSource(consulConfigurationOptions);
            builder.Add(consulConfigurationSource);
            return builder;
        }
        /// <summary>
        /// 添加Consul配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IHostBuilder UseConsul(this IHostBuilder builder, Action<ConsulConfigurationOptions> configure)
        {
            var options = new ConsulConfigurationOptions();
            configure?.Invoke(options);
            return builder.UseConsul(options);
        }
        /// <summary>
        /// 添加Consul配置
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="consulConfigurationOptions"></param>
        /// <returns></returns>
        public static IHostBuilder UseConsul(this IHostBuilder builder, ConsulConfigurationOptions consulConfigurationOptions)
        {
            return builder.ConfigureAppConfiguration((_, cbuilder) => cbuilder.AddConsul(consulConfigurationOptions));
        }
        /// <summary>
        /// 使用consul-template或者watch监听通知(用于拦截http://host:port/consulPath?name=reloadName的请求)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="consulPath"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseConsulWatch(this IApplicationBuilder app, string consulPath = "consul", string method = "POST")
        {
            app.UseMiddleware<ConsulConfigurationMiddleware>(consulPath, method);
            return app;
        }
    }
}
