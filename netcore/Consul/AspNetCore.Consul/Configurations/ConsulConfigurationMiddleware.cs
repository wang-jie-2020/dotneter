using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace AspNetCore.Consul.Configurations
{
    public class ConsulConfigurationMiddleware
    {
        IConfiguration _configuration;
        ILogger _logger;
        string _consulPath = "consul";
        RequestDelegate _next;
        string _httpMethod;

        /// <summary>
        /// 用于拦截http://host:port/consulPath?name=reloadName的请求
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="consulPath"></param>
        public ConsulConfigurationMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory,
            IConfiguration configuration,
            string consulPath,
            string httpMethod)
        {
            _logger = loggerFactory.CreateLogger<ConsulConfigurationMiddleware>();
            _configuration = configuration;
            _consulPath = consulPath;
            _next = next;
            _httpMethod = httpMethod;
        }

        public async Task Invoke(HttpContext context)
        {
            var httpMethod = context.Request.Method;
            var path = context.Request.Path.Value;

            if (string.Equals(httpMethod, _httpMethod, StringComparison.OrdinalIgnoreCase) && Regex.IsMatch(path, $"^/?{Regex.Escape(_consulPath)}/?$"))
            {
                var reloadName = context.Request.Query["name"].ToString() ?? string.Empty;
                Reload(reloadName);
                await Resposed(context.Response);
                return;
            }

            await _next(context);
        }

        private void Reload(string name)
        {
            var isAll = string.IsNullOrEmpty(name);
            var names = name.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var configurationRoot = _configuration as IConfigurationRoot;
            foreach (var provider in configurationRoot.Providers)
            {
                if (provider is ConsulConfigurationProvider consulConfigurationProvider)
                {
                    var reloadName = consulConfigurationProvider.ConsulConfigurationSource.ConsulConfigurationOptions.ReloadName ?? string.Empty;
                    if (isAll || names.Contains(reloadName))
                    {
                        consulConfigurationProvider.Reload();
                        _logger.LogInformation($"reload configuration:{reloadName}");
                    }
                }
            }
        }

        private async Task Resposed(HttpResponse response)
        {
            response.StatusCode = 200;
            response.ContentType = "text/html;charset=utf-8";
            await response.WriteAsync("success", Encoding.UTF8);
        }
    }
}
