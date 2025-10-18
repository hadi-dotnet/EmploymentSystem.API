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
    internal class LikesConfiguration : IEntityTypeConfiguration<Likes>
    {
        public void Configure(EntityTypeBuilder<Likes> builder)
        {
            builder.HasKey(x => x.id);
            builder.Property(x=>x.UserID).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.Property(x=>x.PostID).HasColumnType("INT").IsRequired();
            builder.HasOne(x=>x.User).WithMany(x=>x.Likes).HasForeignKey(x=>x.UserID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x=>x.Post).WithMany(x=>x.Likes).HasForeignKey(x=>x.PostID).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Likes");
        }
    }
}
