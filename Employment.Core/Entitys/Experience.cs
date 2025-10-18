using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class Experience
    {
        public int ID { get; set; }   
        public string? EmployeeID { get; set; }
        public string? CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public string? Title { get; set; }   
        public string? Description { get; set; } 
        public DateTime? StartAT { get; set; } 
        public DateTime? FinishAT { get; set; } 
        public Employees? Employee { get; set; }
        public Company? Company { get; set; }
        public ICollection<ApplyExperience> ApplyExperience { get; set; } = new List<ApplyExperience>();
    }
}
