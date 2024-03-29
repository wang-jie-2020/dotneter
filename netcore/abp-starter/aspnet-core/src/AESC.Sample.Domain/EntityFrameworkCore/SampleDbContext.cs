using AESC.Sample.Entities;
using Microsoft.AspNetCore.Identity;

namespace AESC.Sample.EntityFrameworkCore
{
    [ConnectionStringName(SampleDbProperties.ConnectionStringName)]
    public class SampleDbContext : AbpDbContext<SampleDbContext>
    {
        public DbSet<Book> Books { get; set; }

        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureSample();
        }
    }
}