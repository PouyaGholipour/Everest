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
    public class ReportConfig : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.ReportTitle).IsRequired(true)
                .HasColumnType("nvarchar").HasMaxLength(150);

            builder.Property(r => r.ReportContent).IsRequired(true)
                .HasColumnType("nvarchar(max)");

            builder.HasQueryFilter(r => !r.IsDelete);

        }
    }
}
