using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Hugin.BookStore
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(BookStoreApplicationContractsModule))]
    public class BookStoreHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = BookStoreConsts.Name;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(BookStoreApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
