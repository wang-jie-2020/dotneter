using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Hugin.BookStore.EntityFrameworkCore.EntityTypeConfigurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable(BookStoreConsts.DbProperties.DbTablePrefix + nameof(Book));
            builder.ConfigureByConvention();
        }
    }
}
