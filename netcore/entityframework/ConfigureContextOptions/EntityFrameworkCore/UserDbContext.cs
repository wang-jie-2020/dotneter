using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.EntityFrameworkCore
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureUser();
        }

        public DbSet<User> Users { get; set; }
    }
}
