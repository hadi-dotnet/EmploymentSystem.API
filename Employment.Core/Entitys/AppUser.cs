using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Job.Core.Entitys;
using Job.Infrastructure.Context.Configuration;

namespace Employment.Infrastructure.Entitys
{
    public enum UserTypeEnum
    {
        Company=1,
        Employee=2
    }


    public class AppUser : IdentityUser
    {
        public UserTypeEnum UserType { get; set; }

        public Company Company { get; set; }
        public Employees Employees { get; set; }

        public string? PasswordResetCode { get; set; }

        
        public DateTime? PasswordResetExpiration { get; set; }
        public ICollection< Messages> Messages { get; set; } = new List< Messages>();

        public ICollection<ApplySkill> ApplySkill { get; set; } = new List<ApplySkill>();
        public ICollection<Conversations> Conversations2 { get; set; } = new List<Conversations>();
        public ICollection<Conversations> Conversations1 { get; set; } = new List<Conversations>();
    }
}
