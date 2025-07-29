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
    public class JobsConfiguration : IEntityTypeConfiguration<Jobs>
    {
        public void Configure(EntityTypeBuilder<Jobs> builder)
        {
            builder.HasKey(x => x.ID);
            builder.Property(x=>x.Title).HasColumnType("NVARCHAR").HasMaxLength(50).IsRequired();
            builder.Property(x=>x.Content).HasColumnType("NVARCHAR").HasMaxLength(500).IsRequired();
            builder.Property(x=>x.CompanyID).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.Property(x=>x.SkillsTypeID).HasColumnType("INT").IsRequired(false);
         
            builder.HasOne(x => x.SkillsType).WithMany(x => x.Jobs).HasForeignKey(x => x.SkillsTypeID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Company).WithMany(x => x.Jobs).HasForeignKey(x => x.CompanyID).OnDelete(DeleteBehavior.Restrict);
            builder.Property(x => x.FullTimeORPartTime).HasColumnType("BIT").IsRequired();
            builder.Property(x => x.RemoteOROnSite).HasColumnType("BIT").IsRequired();
            builder.Property(x => x.IsActive).HasColumnType("BIT").IsRequired();
          
            builder.Property(x => x.CreatedAt).HasColumnType("smalldatetime").IsRequired();

            builder.ToTable("Jobs");
           
        }
    }
}
