using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class JobSkillType
    {
        public int id {  get; set; }    
        public int JobID { get; set; }
        public int SkillTypeID { get; set; }      
        public Jobs Job { get; set; }
        public SkillsType SkillsType { get; set; }
    }
}
