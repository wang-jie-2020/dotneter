using Microsoft.AspNetCore.Mvc;
using Refit;

namespace RefitDiscovery.Controllers;

[ApiController]
[Route("[controller]")]
public class UseRefitController : ControllerBase
{
    private readonly IGitHubApi2 _gitHubApi2;
    private readonly IWeatherForecast _weatherForecast;

    public UseRefitController(IGitHubApi2 gitHubApi2, IWeatherForecast weatherForecast)
    {
        _gitHubApi2 = gitHubApi2;
        _weatherForecast = weatherForecast;
    }
    
    [HttpGet("github-user")]
    public async Task<object> GithubUser()
    {
        var gitHubApi = RestService.For<IGitHubApi>("https://api.github.com");
        var octocat = await gitHubApi.GetUser("octocat");
        return octocat;
    }

    [HttpGet("github-user2")]
    public async Task<object> GithubUser2()
    {
        var octocat = await _gitHubApi2.GetUser("octocat");
        return octocat;
    }

    [HttpGet("weatherForecast")]
    public async Task<object> GithubUser3()
    {
        var octocat = await _weatherForecast.GetWeatherForecast();
        return octocat;
    }

    public interface IGitHubApi
    {
        [Get("/users/{user}")]
        [Headers("User-Agent: aspnet")]
        Task<object> GetUser(string user);
    }

    public interface IGitHubApi2
    {
        [Get("/users/{user}")]
        [Headers("User-Agent: aspnet")]
        Task<object> GetUser(string user);
    }

    public interface IWeatherForecast
    {
        [Get("/WeatherForecast")]
        Task<object> GetWeatherForecast();
    }
}