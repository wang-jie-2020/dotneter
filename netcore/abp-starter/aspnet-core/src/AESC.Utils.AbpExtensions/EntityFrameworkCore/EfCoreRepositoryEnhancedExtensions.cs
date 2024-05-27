using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;

namespace AESC.Utils.AbpExtensions.EntityFrameworkCore
{
    public static class EfCoreRepositoryEnhancedExtensions
    {
        public static IServiceCollection AddAbpDbContextHybrid<TDbContext>(
            this IServiceCollection services,
            Action<IAbpDbContextRegistrationOptionsBuilder>? optionsBuilder = null)
            where TDbContext : AbpDbContext<TDbContext>
        {
            //base method
            services.AddAbpDbContext<TDbContext>(optionsBuilder);
            
            //repositories  re-registration
            var options = new AbpDbContextRegistrationOptions(typeof(TDbContext), services);
            optionsBuilder?.Invoke(options);
            new EfCoreRepositoryEnhancedRegistrar(options).AddRepositories();

            return services;
        }
    }
}
