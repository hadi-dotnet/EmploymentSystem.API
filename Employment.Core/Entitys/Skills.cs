using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class Skills
    {
        public int Id { get; set; }
        public string? EmployeeID { get; set; }  
        public int? SkillTypeID { get; set; }
        public SkillsType? SkillsType { get; set; }
        public Employees? Employee {  get; set; }
        public ICollection<ApplySkill> ApplySkill { get; set; } = new List<ApplySkill>();
    }
}
