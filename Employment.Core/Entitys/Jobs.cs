using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public enum EnumFullTimeORPartTime
    {
        FullTime = 1,PartTime = 2
    }

    public enum EnumRemoteOROnSite
    {
        Remote = 1, OnSite = 2
    }
    public class Jobs
    {
        public int ID { get; set; } 
        public string? CompanyID { get; set; } 
        public string? Content { get; set; }
        public string? Title { get; set; }
        public EnumFullTimeORPartTime FullTimeORPartTime { get; set; }
        public EnumRemoteOROnSite RemoteOROnSite { get; set; }
        public bool IsActive { get;set; }
        public DateTime CreatedAt { get; set; }
        public Company? Company { get; set; }
        public ICollection<ApplyJob> ApplyJobs { get; set; } = new List<ApplyJob>();
        public ICollection<JobSkillType> JobSkillTypes { get; set; } = new List<JobSkillType>();
    }
}
