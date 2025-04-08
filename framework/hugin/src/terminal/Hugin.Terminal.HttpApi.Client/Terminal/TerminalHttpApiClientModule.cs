using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Hugin.Terminal
{
    [DependsOn(
        typeof(AbpHttpClientModule),
        typeof(TerminalApplicationContractsModule))]
    public class TerminalHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = TerminalConsts.Name;

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(TerminalApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
