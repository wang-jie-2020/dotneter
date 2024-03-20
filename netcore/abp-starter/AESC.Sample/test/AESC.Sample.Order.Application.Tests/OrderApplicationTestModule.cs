namespace AESC.Sample.Order
{
    [DependsOn(
        typeof(OrderApplicationModule),
        typeof(OrderDomainTestModule)
        )]
    public class OrderApplicationTestModule : AbpModule
    {

    }
}
