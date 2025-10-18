using Job.Services.JobServices.DTOs.SkillsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.EmployeeDTO
{
    public class GetInforamtionDTO
    {
        public string? FirstName { get; set; }
        public string? secoundName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? AboutYou { get; set; }
        public string? UniverCity { get; set; }
        public string? ImagePath { get; set; }

        public List<SkillDTO> Skills { get; set; } = new();
        public List<GetExperinceDTO> Experinces { get; set; } = new();  
    }

    public class SkillDTO
    {
        public int SkillID { get; set; }
        public string? SkillTypeName { get; set; }
        public List<string>? UserApplyed { get; set; } = new();
    }

    public class GetExperinceDTO
    {
        public int ID { get; set; }     
        public string? CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? StartAT { get; set; }
        public string? FinishAT { get; set; }
        public string? IsApplyed { get; set; }
        
    }
   

}
