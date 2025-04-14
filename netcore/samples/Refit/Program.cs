using System.Net;
using Nacos;
using Nacos.AspNetCore.V2;
using Nacos.Extensions.DiscoveryHandler;
using Nacos.V2;
using Polly;
using Polly.Extensions.Http;
using Polly.Fallback;
using Polly.Retry;
using Polly.Timeout;
using Refit;
using RefitDiscovery.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddNacosAspNet(builder.Configuration, section: "Nacos");

#region 弹性故障

// 弹性故障
// AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
//     .HandleTransientHttpError()
//     .Or<TimeoutRejectedException>()
//     .WaitAndRetryAsync(10, _ => TimeSpan.FromMilliseconds(5000));

// IAsyncPolicy policy = Policy.NoOpAsync();
// policy = policy.WrapAsync(Policy.Handle<Exception>().CircuitBreakerAsync(2, TimeSpan.FromMilliseconds(1000)));
// policy = policy.WrapAsync(Policy.TimeoutAsync(() => TimeSpan.FromMilliseconds(1000), Polly.Timeout.TimeoutStrategy.Pessimistic));
// policy = policy.WrapAsync(Policy.Handle<Exception>().WaitAndRetryAsync(5, i => TimeSpan.FromMilliseconds(1000),
//     (async (exception, span, arg3, arg4) =>
//     {
//         Console.WriteLine($"retrying---{arg3}");
//         await Task.CompletedTask;
//     })));
// var policyFallBack = Policy.Handle<Exception>().FallbackAsync(
//     async (ctx, t) =>
//     {
//         await Task.CompletedTask;
//     },
//     async (ex, t) =>
//     {
//         await Task.CompletedTask;
//     });
//
// policy = policyFallBack.WrapAsync(policy);

#endregion

// refit
builder.Services
    .AddRefitClient<UseRefitController.IGitHubApi2>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.github.com"));

// new Uri(builder.Configuration.GetSection("Nacos:ServerAddresses").Get<string[]>()[0]

builder.Services
    .AddRefitClient<UseRefitController.IWeatherForecast>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://app"))
    .ConfigurePrimaryHttpMessageHandler(provider =>
    {
        var svc = provider.GetRequiredService<INacosNamingService>();
        var factory = provider.GetRequiredService<ILoggerFactory>();
        return new NacosDiscoveryHandler(svc, "DEFAULT_GROUP", "DEFAULT", factory);
    })
    .AddHttpMessageHandler(_ => new PollyContextInjectingHandler())
    .AddTransientHttpErrorPolicy(_ => _.RetryAsync(1));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();