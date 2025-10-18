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
    public class ConnectionsConfiguration : IEntityTypeConfiguration<Connections>
    {
        public void Configure(EntityTypeBuilder<Connections> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Sender).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.Property(x => x.Reseiver).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.Property(x => x.Created).HasColumnType("smalldatetime").IsRequired();
            builder.Property(x => x.Status).HasColumnType("INT").IsRequired();
            builder.HasOne(x => x.UserSender).WithMany(x => x.ConnectionSender).HasForeignKey(x => x.Sender).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.UserReseiver).WithMany(x => x.ConnectionReseiver).HasForeignKey(x => x.Reseiver).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Connections");
        }
    }
}
