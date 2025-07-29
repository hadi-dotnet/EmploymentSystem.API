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
    public class ApplyExperienceConfiguration : IEntityTypeConfiguration<ApplyExperience>
    {
        public void Configure(EntityTypeBuilder<ApplyExperience> builder)
        {
            builder.HasKey(x => x.ID);
            builder.Property(x => x.ExperienceID).HasColumnType("int").IsRequired();
            builder.HasOne(x => x.Experience).WithMany(x => x.ApplyExperience).HasForeignKey(x => x.ExperienceID);
          
            builder.ToTable("ApplyExperience");
        }
    }
}
