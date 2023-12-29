using Hugin.BookStore.Stadiums;
using Hugin.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;

namespace Hugin.BookStore.EntityFrameworkCore
{
    [ConnectionStringName(BookStoreConsts.DbProperties.ConnectionStringName)]
    public class BookStoreDbContext : HuginDbContext<BookStoreDbContext>
    {
        #region BookStore

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<BookShop> BookShops { get; set; }

        public DbSet<BookInBookShop> BookInBookShops { get; set; }

        public DbSet<BookShopOwner> BookShopOwners { get; set; }

        #endregion

        public DbSet<Stadium> Stadiums { get; set; }

        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureBookStore();
        }
    }
}