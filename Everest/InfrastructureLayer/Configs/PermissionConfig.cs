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
    public class PermissionConfig : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(p => p.PermissionId);

            builder.HasMany(p => p.Permissions).WithOne().HasForeignKey(p => p.ParentId)
                .IsRequired(false).OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Title).HasMaxLength(80);
        }
    }
}
