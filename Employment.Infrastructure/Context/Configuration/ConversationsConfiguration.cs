using Job.Core.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Infrastructure.Context.Configuration
{
    public class ConversationsConfiguration : IEntityTypeConfiguration<Conversations>
    {
        public void Configure(EntityTypeBuilder<Conversations> builder)
        {
            builder.HasKey(x => x.ID);
            builder.Property(x => x.UserID1).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.Property(x => x.UserID2).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.Property(x => x.CreatedAT).HasDefaultValueSql("GETDATE()").IsRequired();
            builder.HasOne(x=>x.AppUser1).WithMany(x=>x.Conversations1).HasForeignKey(x=>x.UserID1).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x=>x.AppUser2).WithMany(x=>x.Conversations2).HasForeignKey(x=>x.UserID2).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Conversations");
        }
    }
}
