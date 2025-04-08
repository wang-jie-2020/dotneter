using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Hugin.BookStore.EntityFrameworkCore.EntityTypeConfigurations
{
    public class BookShopConfiguration : IEntityTypeConfiguration<BookShop>
    {
        public void Configure(EntityTypeBuilder<BookShop> builder)
        {
            builder.ToTable(BookStoreConsts.DbProperties.DbTablePrefix + nameof(BookShop));
            builder.ConfigureByConvention();
        }
    }
}
