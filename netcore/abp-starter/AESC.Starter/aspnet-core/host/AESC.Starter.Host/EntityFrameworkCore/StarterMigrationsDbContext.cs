using AESC.Sample.EntityFrameworkCore;
using AESC.Starter.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace AESC.Starter.Host.EntityFrameworkCore
{
    public class StarterMigrationsDbContext : AbpDbContext<StarterMigrationsDbContext>
    {
        public StarterMigrationsDbContext(DbContextOptions<StarterMigrationsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureStarter();
            modelBuilder.ConfigureSample();
        }
    }
}
