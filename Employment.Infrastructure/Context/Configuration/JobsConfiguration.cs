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
            builder.Property(x=>x.Content).HasColumnType("NVARCHAR").HasMaxLength(1500).IsRequired();
            builder.Property(x=>x.CompanyID).HasColumnType("NVARCHAR").HasMaxLength(450).IsRequired();
            builder.Property(x => x.FullTimeORPartTime).HasColumnType("INT").IsRequired();
            builder.Property(x => x.RemoteOROnSite).HasColumnType("INT").IsRequired();
            builder.Property(x => x.IsActive).HasColumnType("BIT").IsRequired();         
            builder.Property(x => x.CreatedAt).HasColumnType("datetime").IsRequired();
            builder.HasOne(x => x.Company).WithMany(x => x.Jobs).HasForeignKey(x => x.CompanyID).OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Jobs");          
        }
    }
}
