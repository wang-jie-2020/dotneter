namespace AESC.Sample.Order.EntityFrameworkCore
{
    [ConnectionStringName(OrderDbProperties.ConnectionStringName)]
    public interface IOrderDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
    }
}