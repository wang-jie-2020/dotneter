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
        /// <summary>
        ///  register fluent-api repository
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="options"></param>
        public static void AddConfiguredTypeRepository<TDbContext>(IAbpCommonDbContextRegistrationOptionsBuilder options)
            where TDbContext : AbpDbContext<TDbContext>
        {
            var dbContextType = typeof(TDbContext);

            var configuredGenericTypes = dbContextType.Assembly.GetTypes().Where(type =>
                !type.IsInterface &&
                !type.IsAbstract &&
                !type.IsGenericType
                && ReflectionHelper.IsAssignableToGenericType(type, typeof(IEntityTypeConfiguration<>)));

            foreach (var configuredGenericType in configuredGenericTypes)
            {
                var type = configuredGenericType.GetInterfaces()[0].GenericTypeArguments[0];

                if (type.IsAssignableTo(typeof(IEntity)))
                {
                    options.AddDefaultRepository(type);
                }
            }
        }
    }
}
