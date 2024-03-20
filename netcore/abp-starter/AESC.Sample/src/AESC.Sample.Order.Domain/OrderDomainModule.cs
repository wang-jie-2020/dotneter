namespace AESC.Sample.Order
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(OrderDomainSharedModule),
        typeof(AbpCachingModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpAutoMapperModule)
    )]
    public class OrderDomainModule : AbpModule
    {

    }
}
