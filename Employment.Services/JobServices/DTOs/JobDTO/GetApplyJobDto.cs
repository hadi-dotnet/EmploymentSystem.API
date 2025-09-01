using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.JobDTO
{
    public class GetApplyJobDto
    {
        public int JobID { get; set; }
        public string? EmployeeID { get; set; }
        public string? FullName { get; set; }
    }
}
