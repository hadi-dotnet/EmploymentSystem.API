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
    internal class ExperienceConfigruation : IEntityTypeConfiguration<Experience>
    {
        public void Configure(EntityTypeBuilder<Experience> builder)
        {
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Title).HasColumnType("NVARCHAR").HasMaxLength(50).IsRequired();
            builder.Property(x => x.EmployeeID).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.Property(x => x.Description).HasColumnType("NVARCHAR").HasMaxLength(500).IsRequired();
            builder.Property(x => x.StartAT).HasColumnType("smalldatetime").IsRequired();
            builder.Property(x => x.FinishAT).HasColumnType("smalldatetime").IsRequired(false);
            builder.Property(x => x.CompanyID).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired(false);
            builder.Property(x => x.CompanyName).HasColumnType("NVARCHAR").HasMaxLength(50).IsRequired(false);
            builder.HasOne(x => x.Employee).WithMany(x => x.Experiences).HasForeignKey(x => x.EmployeeID).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Company).WithMany(x => x.Experience).HasForeignKey(x => x.CompanyID).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Experience");
        }
    }
}
