using AspNetCore.Consul.Configurations;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace AspNetCore.Consul.Test
{
    public class ConsulConfigurationTest : BaseUnitTest
    {
        ITestOutputHelper Output;
        public ConsulConfigurationTest(ITestOutputHelper testOutputHelper)
        {
            Output = testOutputHelper;
        }

        /// <summary>
        /// 测试配置加载
        /// </summary>
        [Fact]
        public void ConfigurationTest()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddConsul(options =>
            {
                options.Address = address;
                options.Datacenter = datecenter;
                options.Token = token;
                options.Prefix = "Root/Consul";
            });
            var configuration = builder.Build();

            var sections = configuration.GetChildren();
            Assert.True(sections.Count() > 0);
            foreach (var item in sections)
            {
                Output.WriteLine($"【{item.Path}】={item.Value}");
            }

            Thread.Sleep(10000);//休息10秒，修改Consul中的KV可以测试重新加载

            foreach (var item in sections)
            {
                Output.WriteLine($"【{item.Path}】={item.Value}");
            }
        }
    }
}
