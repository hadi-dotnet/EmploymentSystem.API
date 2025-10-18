using Job.Core.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Job.Infrastructure.Context.Configuration
{
    internal class SkillConfigration : IEntityTypeConfiguration<Skills>
    {
        public void Configure(EntityTypeBuilder<Skills> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.EmployeeID).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.Property(x => x.SkillTypeID).HasColumnType("INT").IsRequired(false);
            builder.HasOne(x => x.Employee).WithMany(x => x.Skills).HasForeignKey(x => x.EmployeeID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.SkillsType).WithMany(x => x.skills).HasForeignKey(x => x.SkillTypeID).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Skills");
        }
    }
}
