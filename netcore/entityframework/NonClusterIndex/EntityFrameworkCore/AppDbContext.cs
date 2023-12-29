using Microsoft.EntityFrameworkCore;

namespace Demo.EntityFrameworkCore
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.Database.SetCommandTimeout(30);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
