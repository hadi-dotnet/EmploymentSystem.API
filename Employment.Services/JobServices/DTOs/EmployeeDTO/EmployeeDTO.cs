using Job.Services.JobServices.DTOs.SkillsDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.EmployeeDTO
{
    public class EmployeeDTO
    {
        [MaxLength(50, ErrorMessage = "First name must be 50 characters or less.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "FirstName must contain letters and spaces only.")]
        public string? FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "SecoundName must contain letters and spaces only.")]
        [MaxLength(50, ErrorMessage = "Secound name must be 50 characters or less.")]
        public string? secoundName { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "LastName must contain letters and spaces only.")]
        [MaxLength(50, ErrorMessage = "Last name must be 50 characters or less.")]
        public string? LastName { get; set; }
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Address must contain letters and spaces only.")]
        [MaxLength(50, ErrorMessage = "Address must be 50 characters or less.")]
        public string? Address { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "About must contain letters and spaces only.")]
        [MaxLength(500, ErrorMessage = "About must be 500 characters or less.")]
        public string? AboutYou { get; set; }


        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "UniverCity must contain letters and spaces only.")]
        [MaxLength(50, ErrorMessage = "UniverCity must be 50 characters or less.")]
        public string? UniverCity { get; set; }

    

    }

}

