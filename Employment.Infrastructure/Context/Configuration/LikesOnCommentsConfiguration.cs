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
    public class LikesOnCommentsConfiguration : IEntityTypeConfiguration<LikesOnComments>
    {
        public void Configure(EntityTypeBuilder<LikesOnComments> builder)
        {
            builder.HasKey(x => x.id);
            builder.Property(x => x.UserID).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.Property(x => x.CommentID).HasColumnType("INT").IsRequired();
            builder.HasOne(x=>x.Comment).WithMany(x=>x.Likes).HasForeignKey(x=>x.CommentID).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x=>x.User).WithMany(x=>x.LikesOnComments).HasForeignKey(x=>x.UserID).OnDelete(DeleteBehavior.Cascade);


            builder.ToTable("LikesOnComments");

        }
    }
}
