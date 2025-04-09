using Refit;

[Headers("User-Agent: Awesome Octocat App")]
public interface IGitHubApi
{
    [Get("/users/{user}")]
    Task<object> GetUser(string user);
}