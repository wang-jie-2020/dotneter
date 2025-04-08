using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureProduct();
        }

        public DbSet<Product> Products { get; set; }
    }
}
