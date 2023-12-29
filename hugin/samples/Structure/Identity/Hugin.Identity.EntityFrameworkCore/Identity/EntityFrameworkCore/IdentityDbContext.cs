using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Hugin.Identity.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class IdentityDbContext : AbpDbContext<IdentityDbContext>
    {
        //public DbSet<AppUser> Users { get; set; }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureHuginIdentity();
        }
    }
}