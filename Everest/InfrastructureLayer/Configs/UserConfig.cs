using DomainLayer.Entities;
using DomainLayer.Enums;
using DomainLayer.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.Configs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                .HasColumnType("nvarchar").HasMaxLength(50);

            builder.Property(x => x.LastName)
                .HasColumnType("nvarchar").HasMaxLength(80);

            builder.Property(x => x.FullName)
                .HasComputedColumnSql("[FirstName] + ' ' + [LastName]")
                .HasColumnType("nvarchar").HasMaxLength(150).IsRequired(false);

            builder.Property(x => x.Email)
                .HasColumnType("varchar").HasMaxLength(150);

            builder.Property(x => x.UserName).HasColumnType("varchar")

                .HasMaxLength(150).IsUnicode(true).IsRequired(true);

            builder.Property(x => x.Password).IsRequired(true);
            builder.Property(x => x.Password).HasMaxLength(20)
                .HasConversion(x => PasswordHash.Base64EnCode(x), x => PasswordHash.Base64DeCode(x));

            builder.Property(x => x.UserType)
                .HasConversion(x => x.ToString(), x => (UserType)Enum.Parse(typeof(UserType), x));

            builder.HasQueryFilter(x => !x.IsDelete);

            builder.Property(x => x.NationalCode).IsRequired(false).IsUnicode(true);

            builder.Property(x => x.UserType).HasDefaultValue(UserType.User);

            //builder.HasOne(x => x.Prog).WithMany(x => x.Users)
            //    .HasForeignKey(x => x.ProgId).OnDelete(DeleteBehavior.Restrict);

            //builder.Property(x=>x.ProgId).IsRequired(false);

        }
    }
}
