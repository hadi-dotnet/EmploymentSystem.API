using Employment.Infrastructure.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class Followers
    {
        public int id { get; set; }
        public string CompanyID { get; set; }   
        public string FollowerID { get; set;}
        public Company Company { get; set; }
        public AppUser UserFolloer { get; set; }
    }
}
