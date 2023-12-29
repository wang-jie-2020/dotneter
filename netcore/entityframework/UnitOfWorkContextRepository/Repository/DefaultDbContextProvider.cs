using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UnitOfWorkContextRepository.Repository
{
    public class DefaultDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultDbContextProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TDbContext> GetDbContextAsync()
        {
            return await Task.FromResult(_serviceProvider.GetRequiredService<TDbContext>());
        }
    }
}