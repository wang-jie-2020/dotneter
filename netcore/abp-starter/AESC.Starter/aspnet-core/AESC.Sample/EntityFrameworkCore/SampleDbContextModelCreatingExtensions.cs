namespace AESC.Sample.EntityFrameworkCore
{
    public static class SampleDbContextModelCreatingExtensions
    {
        public static void ConfigureOrder(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));
        }
    }
}