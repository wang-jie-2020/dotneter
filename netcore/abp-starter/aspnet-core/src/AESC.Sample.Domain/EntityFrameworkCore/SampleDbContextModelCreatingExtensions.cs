namespace AESC.Sample.EntityFrameworkCore
{
    public static class SampleDbContextModelCreatingExtensions
    {
        public static void ConfigureSample(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            //builder.ApplyConfigurationsFromAssembly(typeof(SampleDbContext).Assembly);
        }
    }
}