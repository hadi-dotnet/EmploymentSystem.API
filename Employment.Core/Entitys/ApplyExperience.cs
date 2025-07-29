using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class ApplyExperience
    {
        public int ID { get; set; }
        public int ExperienceID { get; set;} 
        public Experience? Experience { get; set; }
    }
}
