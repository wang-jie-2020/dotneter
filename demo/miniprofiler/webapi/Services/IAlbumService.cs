using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Models;

namespace Demo.Services
{
    public interface IAlbumService
    {
        Task<List<Album>> GetAllAsync();

        Task<Album> GetByIdAsync(int id);

        Task<Album> AddAsync(Album model);

        Task UpdateAsync(Album model);

        Task DeleteAsync(Album model);
    }
}
