using Lion.AbpPro.BasicManagement.EntityFrameworkCore;
using Lion.AbpPro.DataDictionaryManagement.EntityFrameworkCore;
using Lion.AbpPro.LanguageManagement.EntityFrameworkCore;
using Lion.AbpPro.NotificationManagement.EntityFrameworkCore;

namespace AESC.Shared.EntityFrameworkCore
{
    public static class SharedDbContextModelCreatingExtensions
    {
        public static void ConfigureShared(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ConfigureBasicManagement();
            builder.ConfigureNotificationManagement();
            builder.ConfigureDataDictionaryManagement();
            builder.ConfigureLanguageManagement();
            builder.ApplyConfigurationsFromAssembly(typeof(SharedDbContext).Assembly);
        }
    }
}