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
        /// �������ü���
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
                Output.WriteLine($"��{item.Path}��={item.Value}");
            }

            Thread.Sleep(10000);//��Ϣ10�룬�޸�Consul�е�KV���Բ������¼���

            foreach (var item in sections)
            {
                Output.WriteLine($"��{item.Path}��={item.Value}");
            }
        }
    }
}
