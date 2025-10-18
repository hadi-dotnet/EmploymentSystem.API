using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.SkillsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.JobDTO
{
    public class GetJobsDTO
    {
        public int JobID { get; set; }  
        public string? CompanyID { get; set; }
       public string? CompanyName { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public List<SkillsTypeDTO>? SkillTypeName { get; set; }   
        public string FullTimeORPartTime { get; set; }       
        public string RemoteOROnSite { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
