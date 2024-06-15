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
    public class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CourseTitle).IsRequired()
                .HasMaxLength(150).HasColumnType("nvarchar");

            builder.Property(c => c.Description).IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(c => c.CoachName).IsRequired(false)
                .HasMaxLength(100).HasColumnType("nvarchar");

            builder.HasQueryFilter(c => !c.IsDelete);

            builder.Property(c => c.CourseType).HasDefaultValue(CourseType.Mountaineering);

            builder.HasOne(c => c.Category)
                .WithMany(c => c.Courses).HasForeignKey(c => c.CategoryId);
        }
    }
}
