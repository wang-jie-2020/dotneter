using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Demo.EntityFrameworkCore;

public class HostMigrationsDbContext : AbpDbContext<HostMigrationsDbContext>
{
    public HostMigrationsDbContext(DbContextOptions<HostMigrationsDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureDemo();
    }
}
