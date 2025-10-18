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
    public class CommentsConfiguration : IEntityTypeConfiguration<Comments>
    {
        public void Configure(EntityTypeBuilder<Comments> builder)
        {
            builder.HasKey(x => x.id);
            builder.Property(x => x.UserID).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.Property(x => x.PostID).HasColumnType("INT").IsRequired();
            builder.Property(x => x.Comment).HasColumnType("NVARCHAR").HasMaxLength(500).IsRequired();
            builder.Property(x => x.CreatedAt).HasColumnType("smalldatetime").IsRequired();
            builder.HasOne(x=>x.User).WithMany(x=>x.Comments).HasForeignKey(x=>x.UserID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x=>x.Post).WithMany(x=>x.Comments).HasForeignKey(x=>x.PostID).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Comments");

        }
    }
}
