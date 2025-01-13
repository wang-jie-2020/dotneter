using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace pythoncsharp
{
    [DependsOn(
        typeof(AbpAutofacModule)
    )]
    internal class CoreModule : AbpModule
    {
        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
            EmbeddedPython.Run(configuration);
        }
    }
}
