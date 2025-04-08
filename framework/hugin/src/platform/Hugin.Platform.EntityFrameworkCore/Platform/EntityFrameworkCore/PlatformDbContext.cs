using Hugin.EntityFrameworkCore;
using Hugin.Platform.Stadiums;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;

namespace Hugin.Platform.EntityFrameworkCore
{
    [ConnectionStringName(PlatformConsts.DbProperties.ConnectionStringName)]
    public class PlatformDbContext : HuginDbContext<PlatformDbContext>
    {
        public DbSet<Stadium> Stadiums { get; set; }

        public PlatformDbContext(DbContextOptions<PlatformDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigurePlatform();
        }
    }
}