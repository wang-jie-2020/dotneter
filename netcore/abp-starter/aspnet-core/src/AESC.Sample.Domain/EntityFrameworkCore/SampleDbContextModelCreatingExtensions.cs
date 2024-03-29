using Volo.Abp.Users.EntityFrameworkCore;

namespace AESC.Sample.EntityFrameworkCore
{
    public static class SampleDbContextModelCreatingExtensions
    {
        public static void ConfigureSample(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.Entity<AppUser>(b =>
            {
                b.ToTable("AbpUsers");
                b.ConfigureByConvention();
                b.ConfigureAbpUser();
            });

            builder.ApplyConfigurationsFromAssembly(typeof(SampleDbContext).Assembly);
        }
    }
}