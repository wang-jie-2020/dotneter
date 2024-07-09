using Autofac.Features.OwnedInstances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AESC.Starter.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AESC.Starter.EntityFrameworkCore.EntityConfigurations
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
