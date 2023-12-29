using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Consul
{
    public class ConsulServiceOptions
    {
        /// <summary>
        /// 服务Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 服务地址
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 服务端口
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 服务标签
        /// </summary>
        public string[] Tags { get; set; }
        /// <summary>
        /// 健康检查地址，默认http(s)://host:port/,grpc是host:port
        /// </summary>
        public string HealthCheckUrl { get; set; }
        /// <summary>
        /// 健康检查的路由，即http(s)://host:port/{HealthCheckPath}，指定了HealthCheckUrl，HealthCheckPath将失效
        /// </summary>
        public string HealthCheckPath { get; set; }
        /// <summary>
        /// 健康检查是否使用https
        /// </summary>
        public bool HealthCheckUseHttps { get; set; }
        /// <summary>
        /// 使用Grpc做健康检查，地址是HealthCheckUrl
        /// </summary>
        public bool HealthCheckUseGrpc { get; set; }
        /// <summary>
        /// 健康检查间隔，默认10秒
        /// </summary>
        public TimeSpan? HealthCheckInterval { get; set; }
    }
}
