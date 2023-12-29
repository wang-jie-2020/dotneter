using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;

namespace Demo
{
    public class HelloService : IHelloService, ITransientDependency
    {
        private readonly ILogger<HelloService> _logger;

        public HelloService(ILogger<HelloService> logger)
        {
            _logger = logger;
        }

        public Task SayHelloAsync()
        {
            _logger.LogInformation("Hello World!");
            return Task.CompletedTask;
        }
    }
}
