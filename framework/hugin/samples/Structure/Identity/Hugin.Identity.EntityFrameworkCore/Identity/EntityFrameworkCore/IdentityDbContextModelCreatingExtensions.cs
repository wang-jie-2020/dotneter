using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Hugin.Identity.EntityFrameworkCore
{
    public static class IdentityDbContextModelCreatingExtensions
    {
        public static void ConfigureHuginIdentity(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            //builder.Entity<AppUser>(b =>
            //{
            //    b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users");
            //    b.ConfigureByConvention();
            //    b.ConfigureAbpUser();
            //});
        }
    }
}