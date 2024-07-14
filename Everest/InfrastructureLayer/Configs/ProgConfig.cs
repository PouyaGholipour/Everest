using DomainLayer.Entities;
using DomainLayer.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Configs
{
    public class ProgConfig : IEntityTypeConfiguration<Prog>
    {
        public void Configure(EntityTypeBuilder<Prog> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title).IsRequired(true)
                .HasColumnType("nvarchar").HasMaxLength(150);

            builder.Property(p => p.Description).IsRequired(true)
                .HasColumnType("nvarchar(max)");

            builder.Property(p => p.LeaderName)
                .HasColumnType("nvarchar").HasMaxLength(100);

            builder.HasQueryFilter(p => !p.IsDelete);

            builder.Property(p => p.ProgramType).HasDefaultValue(ProgramType.Easy);

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Progs).HasForeignKey(p => p.CategoryId);

        }
    }
}
