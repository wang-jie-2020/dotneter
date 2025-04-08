using System.Threading;
using System.Threading.Tasks;
using Hugin.BookStore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace Hugin
{
    public class HostedService : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var application = AbpApplicationFactory.Create<BookStoreHttpApiClientTestModule>())
            {
                application.Initialize();

                var client = application.ServiceProvider.GetRequiredService<BookStoreClientService>();
                await client.RunAsync();

                application.Shutdown();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
