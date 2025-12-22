using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace I18n.LocalizationExtensions.Tests;

public class IntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    public WebApplicationFactory<Program> _factory;

    public IntegrationTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;

        _factory = factory.WithWebHostBuilder(builder =>
        {
            //builder.ConfigureTestServices(services =>
            //{
            //    // 替换真实服务为测试服务
            //    services.RemoveAll<IDatabaseService>();
            //    services.AddSingleton<IDatabaseService, TestDatabaseService>();
            //    
            //    // 配置测试数据库
            //    services.RemoveAll<DbContextOptions<AppDbContext>>();
            //    services.AddDbContext<AppDbContext>(options =>
            //        options.UseInMemoryDatabase("TestDb"));
            //});
        });
    }

    [Fact]
    public async Task GetDemo_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/demo");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetDemo_ZH()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept-Language", "zh-cn");
        var response = await client.GetAsync("/demo");

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("你好", content);
    }
    
    [Fact]
    public async Task GetDemo_EN()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept-Language", "en-us");
        var response = await client.GetAsync("/demo");

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("Hello", content);
    }
    
    [Fact]
    public async Task GetDemo_FR()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Accept-Language", "fr");
        var response = await client.GetAsync("/demo");

        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal("bonjour", content);
    }
}