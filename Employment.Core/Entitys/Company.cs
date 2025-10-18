using Employment.Infrastructure.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class Company
    {
        public string? UserID { get; set; } 
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? About { get; set; }
        public string? Image { get; set; }
        public AppUser? AppUser { get; set; }
        public ICollection<Jobs> Jobs { get; set; } = new List<Jobs>();
        public ICollection<Experience> Experience { get; set; } = new List<Experience>();
        public ICollection<Followers> Followers { get; set; } = new List<Followers>();  
    }
}
