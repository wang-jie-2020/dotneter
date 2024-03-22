namespace AESC.Sample.EntityFrameworkCore
{
    [ConnectionStringName(SampleDbProperties.ConnectionStringName)]
    public class SampleDbContext : AbpDbContext<SampleDbContext>
    {
        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureOrder();
        }
    }
}