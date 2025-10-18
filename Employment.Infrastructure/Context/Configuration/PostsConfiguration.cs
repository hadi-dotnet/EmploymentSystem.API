using Job.Core.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Infrastructure.Context.Configuration
{
    public class PostsConfiguration : IEntityTypeConfiguration<Posts>
    {
        public void Configure(EntityTypeBuilder<Posts> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserID).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.Property(x => x.Content).HasColumnType("NVARCHAR").HasMaxLength(600).IsRequired(false);
            builder.Property(x => x.Image).HasColumnType("NVARCHAR").HasMaxLength(500).IsRequired(false);
            builder.Property(x => x.CreatedAt).HasColumnType("smalldatetime").IsRequired();
            builder.HasOne(x=>x.User).WithMany(x=>x.Posts).HasForeignKey(x=>x.UserID).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Posts");
        }
    }
}
