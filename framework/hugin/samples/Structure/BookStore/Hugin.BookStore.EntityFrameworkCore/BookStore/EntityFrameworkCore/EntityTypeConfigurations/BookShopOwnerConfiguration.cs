using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Hugin.BookStore.EntityFrameworkCore.EntityTypeConfigurations
{
    public class BookShopOwnerConfiguration : IEntityTypeConfiguration<BookShopOwner>
    {
        public void Configure(EntityTypeBuilder<BookShopOwner> builder)
        {
            builder.ToTable(BookStoreConsts.DbProperties.DbTablePrefix + nameof(BookShopOwner));
            builder.ConfigureByConvention();
        }
    }
}
