namespace AESC.Sample.Order
{
    [DependsOn(
        typeof(OrderDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class OrderApplicationContractsModule : AbpModule
    {

    }
}
