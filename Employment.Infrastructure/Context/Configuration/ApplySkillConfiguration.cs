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
    public class ApplySkillConfiguration : IEntityTypeConfiguration<ApplySkill>
    {
        public void Configure(EntityTypeBuilder<ApplySkill> builder)
        {
            builder.HasKey(x => x.ID);
            builder.Property(x => x.SkillID).HasColumnType("INT").IsRequired();
            builder.Property(x => x.UserID).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.HasOne(x=>x.Skill).WithMany(x => x.ApplySkill).HasForeignKey(x=>x.SkillID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x=>x.AppUser).WithMany(x => x.ApplySkill).HasForeignKey(x=>x.UserID).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("ApplySkill");
        }
    }
}
