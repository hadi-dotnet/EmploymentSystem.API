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
    public class JobSkillTypeConfiguration : IEntityTypeConfiguration<JobSkillType>
    {
        public void Configure(EntityTypeBuilder<JobSkillType> builder)
        {
            builder.HasKey(x => x.id);
            builder.Property(x => x.JobID).HasColumnType("INT").IsRequired();
            builder.Property(x => x.SkillTypeID).HasColumnType("INT").IsRequired();
            builder.HasOne(x=>x.Job).WithMany(x=>x.JobSkillTypes).HasForeignKey(x=>x.JobID).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x=>x.SkillsType).WithMany(x=>x.JobSkillType).HasForeignKey(x=>x.SkillTypeID).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("JobSkillType");
        }
    }
}
