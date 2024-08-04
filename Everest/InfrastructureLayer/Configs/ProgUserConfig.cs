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
    public class ProgUserConfig : IEntityTypeConfiguration<ProgUser>
    {
        public void Configure(EntityTypeBuilder<ProgUser> builder)
        {
            builder.HasKey(x => new { x.UserId, x.ProgId });

            builder.HasOne(x => x.Prog)
                .WithMany(x => x.ProgUsers)
                .HasForeignKey(x => x.ProgId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(x => x.ProgUsers)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
