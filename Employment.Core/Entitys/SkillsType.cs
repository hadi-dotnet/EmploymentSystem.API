using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class SkillsType
    {
        public int id {  get; set; }
        public string? TypeName{  get; set; }
        public ICollection<Skills> skills { get; set; } =new List<Skills>();
        public ICollection<JobSkillType> JobSkillType { get; set; } = new List<JobSkillType>();

    }
}
