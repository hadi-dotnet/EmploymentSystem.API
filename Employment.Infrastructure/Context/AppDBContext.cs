using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Emit;
using Employment.Infrastructure.Entitys;
using Employment.Core.Entitys;
using Job.Core.Entitys;
using Job.Infrastructure.Context.Configuration;
using Job.Core.Helper;

namespace Employment.Infrastructure.Context
{
    public class AppDBContext:IdentityDbContext<AppUser>
    {
          
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<ApplyExperience> ApplyExperience { get; set; }
        public DbSet<ApplyJob> ApplyJob { get; set; }
        public DbSet<ApplySkill> ApplySkill { get; set; }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Conversations> Conversations { get; set; }
        public DbSet<Experience> Experience { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<SkillsType> SkillsType { get; set; }
        public DbSet<Jobs> Jobs { get; set; }
        public DbSet<Connections> Connections { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<LikesOnComments> LikesOnComments { get; set; }
        public DbSet<Followers> Followers { get; set; }
        public DbSet<JobSkillType> JobSkillType { get; set; }


        public AppDBContext(DbContextOptions<AppDBContext> option) :base (option)
        {
            
        }

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<SkillsType>().HasData(SkillList.GetSkillList());
                
            builder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
        }
      
    }
}
