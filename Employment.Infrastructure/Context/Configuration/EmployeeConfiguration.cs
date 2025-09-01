using Employment.Core.Entitys;
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
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employees>
    {
        public void Configure(EntityTypeBuilder<Employees> builder)
        {
            builder.HasKey(x => x.UserID);
            builder.Property(x => x.FirstName).HasColumnType("NVARCHAR").HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.LastName).HasColumnType("NVARCHAR").HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.Address).HasColumnType("NVARCHAR").HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.AboutYou).HasColumnType("NVARCHAR").HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.UniverCity).HasColumnType("NVARCHAR").HasMaxLength(50).IsRequired(false);
            builder.HasOne(x=>x.AppUser).WithOne(x=>x.Employees).HasForeignKey<Employees>(x=>x.UserID);
            builder.Property(x => x.Image).HasColumnType("NVARCHAR").HasMaxLength(500).IsRequired(false);
            
            builder.ToTable("Employees");
        }
    }
}
