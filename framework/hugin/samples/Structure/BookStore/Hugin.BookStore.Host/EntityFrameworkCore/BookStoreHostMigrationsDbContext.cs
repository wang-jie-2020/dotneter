using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Hugin.BookStore.EntityFrameworkCore
{
    public class BookStoreHostMigrationsDbContext : AbpDbContext<BookStoreHostMigrationsDbContext>
    {
        public BookStoreHostMigrationsDbContext(DbContextOptions<BookStoreHostMigrationsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureBookStore();
        }
    }
}