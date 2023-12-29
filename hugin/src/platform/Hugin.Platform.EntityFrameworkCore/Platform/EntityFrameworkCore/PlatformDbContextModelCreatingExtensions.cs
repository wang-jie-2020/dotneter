using Hugin.Domain.Entities.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Volo.Abp;

namespace Hugin.Platform.EntityFrameworkCore
{
    public static class PlatformDbContextModelCreatingExtensions
    {
        public static void ConfigurePlatform(
            this ModelBuilder builder,
            Action<PlatformModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new PlatformModelBuilderConfigurationOptions(
                PlatformConsts.DbProperties.DbTablePrefix,
                PlatformConsts.DbProperties.DbSchema);

            optionsAction?.Invoke(options);

            //configurations
            builder.ApplyConfigurationsFromAssembly(typeof(PlatformDbContextModelCreatingExtensions).Assembly);

            //decimal
            foreach (var model in builder.Model.GetEntityTypes())
            {
                var properties = model.ClrType.GetProperties()
                    .Where(c => c.IsDefined(typeof(DecimalPrecisionAttribute), true));

                foreach (var prop in properties)
                {
                    var attr = prop.GetCustomAttribute<DecimalPrecisionAttribute>();
                    builder.Entity(model.ClrType).Property(prop.Name).HasColumnType($"decimal({attr.Precision},{attr.Scale})");
                }
            }

            //comment
            foreach (var model in builder.Model.GetEntityTypes())
            {
                var properties = model.ClrType.GetProperties()
                    .Where(c => c.IsDefined(typeof(DescriptionAttribute), true));

                foreach (var prop in properties)
                {
                    if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                    {
                        continue;
                    }

                    var attr = prop.GetCustomAttribute<DescriptionAttribute>();
                    builder.Entity(model.ClrType).Property(prop.Name).HasComment(attr.Description);
                }
            }
        }
    }
}