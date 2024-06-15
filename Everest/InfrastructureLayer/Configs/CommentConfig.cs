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
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title).IsRequired(true)
                .HasColumnType("nvarchar").HasMaxLength(150);

            builder.Property(c => c.CommentDescription).IsRequired(true)
                .HasColumnType("nvarchar").HasMaxLength(500);

            builder.HasQueryFilter(c => !c.IsDelete);
            builder.Property(c => c.CommentStatus).HasDefaultValue(CommentStatus.WaitConfirmation);

            builder.HasOne(c => c.User).WithMany(c => c.Comments)
                .HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.Report).WithMany(c => c.Comments).HasForeignKey(c => c.ReportId);
            builder.HasOne(c => c.Course).WithMany(c => c.Comments).HasForeignKey(c => c.CourseId);
        }
    }
}
