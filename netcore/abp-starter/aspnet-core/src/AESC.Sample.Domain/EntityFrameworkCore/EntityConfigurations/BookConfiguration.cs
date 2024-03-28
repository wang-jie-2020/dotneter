using Autofac.Features.OwnedInstances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AESC.Sample.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AESC.Sample.EntityFrameworkCore.EntityConfigurations
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
