using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Consul.Configurations
{
    public class ConsulConfigurationOptions
    {
        /// <summary>
        /// 热加载模式
        /// </summary>
        public WatchMode Mode { get; set; } = WatchMode.Poll;
        /// <summary>
        /// 阻塞查询模式时加载延迟(十分钟以内，默认5分钟)
        /// </summary>
        public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(5);
        /// <summary>
        /// 使用consul-template或者watch实现热加载时通知的路由名称
        /// </summary>
        public string ReloadName { get; set; }
        /// <summary>
        /// Consul注册中心地址（http://host:port）
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Api Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// 是否移除前缀
        /// </summary>
        public bool RemovePrefix { get; set; } = true;
        /// <summary>
        /// 数据中心
        /// </summary>
        public string Datacenter { get; set; } = "dc1";
        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
    public enum WatchMode
    {
        /// <summary>
        /// 不使用热加载
        /// </summary>
        None = 0,
        /// <summary>
        /// 使用阻塞查询实现热加载
        /// </summary>
        Poll = 1,
        /// <summary>
        /// 使用consul-template或者watch实现热加载
        /// </summary>
        Watch = 2
    }
}
