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
    internal class SkillsTypeConfigruation : IEntityTypeConfiguration<SkillsType>
    {
        public void Configure(EntityTypeBuilder<SkillsType> builder)
        {
            builder.HasKey(x => x.id);
            builder.Property(x => x.id).ValueGeneratedNever();
            builder.Property(x=>x.TypeName).HasColumnType("NVARCHAR").HasMaxLength(50).IsRequired();

            builder.ToTable("SkillsType");
        }
    }
}
