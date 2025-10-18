using Job.Core.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Infrastructure.Context.Configuration
{
    public class FollowersConfiguration : IEntityTypeConfiguration<Followers>
    {
        public void Configure(EntityTypeBuilder<Followers> builder)
        {
            builder.HasKey(x => x.id);
            builder.Property(x => x.CompanyID).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.Property(x => x.FollowerID).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.HasOne(x => x.Company).WithMany(x => x.Followers).HasForeignKey(x => x.CompanyID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.UserFolloer).WithMany(x => x.Followers).HasForeignKey(x => x.FollowerID).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Followers");
        }
    }
}
