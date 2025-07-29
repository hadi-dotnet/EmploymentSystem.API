using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.JobDTO
{
    public class UpdateJobDTO
    {
        [Required]
        public int JobID { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Content must be 500 characters or less.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Content must contain letters and spaces only.")]
        public string? Content { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Title must be 50 characters or less.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Title must contain letters and spaces only.")]
        public string? Title { get; set; }

        [Required]
        public int? JobTypeID { get; set; }

        [Required]
        public bool FullTimeORPartTime { get; set; }

        [Required]
        public bool RemoteOROnSite { get; set; }

        [Required]
        public bool IsActive { get; set; }  
      
    }
}
