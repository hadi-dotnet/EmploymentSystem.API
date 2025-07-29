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
    public class ApplyJobConfiguration : IEntityTypeConfiguration<ApplyJob>
    {
        public void Configure(EntityTypeBuilder<ApplyJob> builder)
        {
            builder.HasKey(x => x.ID);
            builder.Property(x => x.JobID).HasColumnType("INT").IsRequired(false);
            builder.Property(x => x.EmployeeID).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired(false);
            builder.HasOne(x=>x.Employee).WithMany(x=>x.ApplyJobs).HasForeignKey(x=>x.EmployeeID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x=>x.Jobs).WithMany(x=>x.ApplyJobs).HasForeignKey(x=>x.JobID).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("ApplyJob");
        }
    }
}
