using abp_outer_module;
using Microsoft.Extensions.Hosting;

namespace abp_console;

public class ConsoleAppHostedService : IHostedService
{
    private readonly HelloWorldService _helloWorldService;

    public ConsoleAppHostedService(HelloWorldService helloWorldService)
    {
        _helloWorldService = helloWorldService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _helloWorldService.SayHelloAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
