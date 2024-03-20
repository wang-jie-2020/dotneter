namespace AESC.Sample.Order
{
    [DependsOn(
        typeof(OrderApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class OrderHttpApiClientModule : AbpModule
    {
        public const string RemoteServiceName = "Order";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
                typeof(OrderApplicationContractsModule).Assembly,
                RemoteServiceName
            );
        }
    }
}
