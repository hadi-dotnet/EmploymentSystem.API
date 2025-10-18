using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Infrastructure.Context.Configuration
{


    public class MessagesConfiguration : IEntityTypeConfiguration<Messages>
    {
        public void Configure(EntityTypeBuilder<Messages> builder)
        {
            builder.HasKey(x => x.ID);
            builder.Property(x => x.MassageText).HasColumnType("NVARCHAR").HasMaxLength(500).IsRequired();
            builder.Property(x => x.ConversationId).HasColumnType("INT").IsRequired();
            builder.Property(x => x.SendAT).HasColumnType("datetime2").IsRequired();
            builder.Property(x => x.SenderBy).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.HasOne(x => x.AppUser).WithMany(x => x.Messages).HasForeignKey(x => x.SenderBy).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x=>x.Conversation).WithMany(x=>x.Message).HasForeignKey(x=>x.ConversationId).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Messages");
        }
    }
}
