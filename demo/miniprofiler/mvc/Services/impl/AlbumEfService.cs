using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Data;
using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.Services.impl
{
    public class AlbumEfService : IAlbumService
    {
        private readonly ApplicationDbContext _context;

        public AlbumEfService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Album>> GetAllAsync()
        {
            return await _context.Albums.ToListAsync();
        }

        public async Task<Album> GetByIdAsync(int id)
        {
            return await _context.Albums.FindAsync(id);
        }

        public async Task<Album> AddAsync(Album model)
        {
            _context.Albums.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task UpdateAsync(Album model)
        {
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Album model)
        {
            _context.Albums.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
