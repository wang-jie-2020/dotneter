using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Reflection;

namespace AESC.Utils.AbpExtensions.EntityFrameworkCore
{
    public static class EntityFrameworkCoreRepositoriesUtils
    {
        public static void AddDefaultRepositoriesOfFluentSyntax<TDbContext>(
            IAbpCommonDbContextRegistrationOptionsBuilder options) where TDbContext : AbpDbContext<TDbContext>
        {
            var dbContextType = typeof(TDbContext);

            var fluentTypes = dbContextType.Assembly.GetTypes().Where(type =>
                    !type.IsInterface && !type.IsAbstract && !type.IsGenericType
                    && ReflectionHelper.IsAssignableToGenericType(type, typeof(IEntityTypeConfiguration<>))
                    && type.GetInterfaces()[0].IsGenericType
                    && typeof(IEntity).IsAssignableFrom(type.GetInterfaces()[0].GenericTypeArguments[0]))
                .Select(t => t.GetInterfaces()[0].GenericTypeArguments[0]);

            foreach (var type in fluentTypes)
            {
                options.AddDefaultRepository(type);
            }
        }
    }
}
