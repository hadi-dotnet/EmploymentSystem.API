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
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(x => x.UserID);
            builder.Property(x => x.Address).HasColumnType("NVARCHAR").HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.Image).HasColumnType("NVARCHAR").HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.About).HasColumnType("NVARCHAR").HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.Name).HasColumnType("NVARCHAR").HasMaxLength(50).IsRequired(false);
            builder.HasOne(x => x.AppUser).WithOne(x => x.Company).HasForeignKey<Company>(x => x.UserID);
           
            builder.ToTable("Companys");
        }
    }
}
