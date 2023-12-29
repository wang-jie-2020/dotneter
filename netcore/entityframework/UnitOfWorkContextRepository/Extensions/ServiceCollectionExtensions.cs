using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UnitOfWorkContextRepository.Repository;
using UnitOfWorkContextRepository.Transaction;

namespace UnitOfWorkContextRepository.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnitOfWorkRepository<TDbContext>(this IServiceCollection services) where TDbContext : DbContext
        {
            services.AddTransient(typeof(IEfCoreRepository<>), typeof(EfCoreRepository<>));
            services.AddTransient(typeof(IEfCoreRepository), typeof(EfCoreRepository<TDbContext>));
            services.AddTransient(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IUnitOfWorkAccessor, UnitOfWorkAccessor>();
            services.AddSingleton<IUnitOfWorkManager, UnitOfWorkManager>();

            //在默认IOC的情况下,正常DEMO不报错,单元测试报错
            services.Configure<UnitOfWorkDefaultOptions>(options => { });

            return services;
        }
    }
}
