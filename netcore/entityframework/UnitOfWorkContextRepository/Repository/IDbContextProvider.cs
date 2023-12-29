using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UnitOfWorkContextRepository.Repository
{
    public interface IDbContextProvider<TDbContext> where TDbContext : DbContext
    {
        Task<TDbContext> GetDbContextAsync();
    }
}
