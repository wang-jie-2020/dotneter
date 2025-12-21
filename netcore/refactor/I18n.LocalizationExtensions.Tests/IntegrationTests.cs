using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace I18n.LocalizationExtensions.Tests;

public class IntegrationTests: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        
        // _factory = factory.WithWebHostBuilder(builder =>
        // {
        //     builder.ConfigureTestServices(services =>
        //     {
        //         // 替换真实服务为测试服务
        //         services.RemoveAll<IDatabaseService>();
        //         services.AddSingleton<IDatabaseService, TestDatabaseService>();
        //         
        //         // 配置测试数据库
        //         services.RemoveAll<DbContextOptions<AppDbContext>>();
        //         services.AddDbContext<AppDbContext>(options =>
        //             options.UseInMemoryDatabase("TestDb"));
        //     });
        // });
    }

    [Fact]
    public async Task GetGreeting_ReturnsOk()
    {
        //  Arrange
        var client = _factory.CreateClient();

        //  Act
        var response = await client.GetAsync("/demo");

        //  Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}