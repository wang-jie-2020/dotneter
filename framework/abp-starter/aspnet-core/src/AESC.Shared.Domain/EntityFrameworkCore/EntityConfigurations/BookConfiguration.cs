using AESC.Shared.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AESC.Shared.EntityFrameworkCore.EntityConfigurations
{
    internal class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable(nameof(Book));
            builder.ConfigureByConvention();
        }
    }
}
