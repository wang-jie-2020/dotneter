using Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.EntityFrameworkCore
{
    public static class DbContextModelCreatingExtensions
    {
        public static void ConfigureUser(this ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("user");
            builder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
        }

        public static void ConfigureOrder(this ModelBuilder builder)
        {
            builder.Entity<Order>().ToTable("order");
            builder.Entity<Order>().Property(p => p.Name).HasMaxLength(100);
            builder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
        }

        public static void ConfigureProduct(this ModelBuilder builder)
        {
            builder.Entity<Product>().ToTable("product");
            builder.Entity<Product>().Property(p => p.Name).HasMaxLength(100);
            builder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);
        }
    }
}
