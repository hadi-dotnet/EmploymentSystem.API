using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class Jobs
    {
        public int ID { get; set; } 
        public string? CompanyID { get; set; } 
        public string? Content { get; set; }
        public string? Title { get; set; }
        public int? SkillsTypeID { get; set; }
        public bool FullTimeORPartTime { get; set; }
        public bool RemoteOROnSite { get; set; }
        public bool IsActive { get;set; }
        public DateTime CreatedAt { get; set; }
        public Company? Company { get; set; }
        public SkillsType? SkillsType { get; set; }
        public ICollection<ApplyJob> ApplyJobs { get; set; } = new List<ApplyJob>();
    }
}
