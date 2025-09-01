using Employment.Infrastructure.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class ApplySkill
    {
        public int ID { get; set; }
        public int SkillID { get; set; }
        public string UserID { get; set; }
        public Skills Skill { get; set; }
        public AppUser AppUser { get; set; }    
    }
}
