using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Configs
{
    public class JournalConfig : IEntityTypeConfiguration<Journal>
    {
        public void Configure(EntityTypeBuilder<Journal> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(j => j.JournalTitle).IsRequired(true)
                .HasColumnType("nvarchar").HasMaxLength(150);

            builder.Property(j => j.Content).IsRequired(true)
                .HasColumnType("nvarchar(max)");

            builder.HasQueryFilter(j => !j.IsDelete);
        }
    }
}
