namespace AESC.Sample.Order.EntityFrameworkCore
{
    [DependsOn(
        typeof(OrderDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class OrderEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<OrderDbContext>(options =>
            {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
            });
        }
    }
}