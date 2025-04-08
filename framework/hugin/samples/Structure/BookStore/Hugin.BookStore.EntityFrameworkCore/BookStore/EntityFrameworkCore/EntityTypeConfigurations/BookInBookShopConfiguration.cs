using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Hugin.BookStore.EntityFrameworkCore.EntityTypeConfigurations
{
    public class BookInBookShopConfiguration : IEntityTypeConfiguration<BookInBookShop>
    {
        public void Configure(EntityTypeBuilder<BookInBookShop> builder)
        {
            builder.ToTable(BookStoreConsts.DbProperties.DbTablePrefix + nameof(BookInBookShop));
            builder.ConfigureByConvention();
        }
    }
}
