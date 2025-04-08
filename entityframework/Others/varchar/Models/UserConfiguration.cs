using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Models
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

            builder.Property(x => x.Name2).HasColumnType("varchar(100)");
            builder.Property(x => x.Name3).HasColumnType("nvarchar(100)");
        }
    }
}
