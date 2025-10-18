using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.ExperinceDTO
{
    public class ExperinceDTO
    {
        [Required]
        public string? CompanyNameorID { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Title must be 50 characters or less.")]
        public string? Title { get; set; } 

        [Required]
        [MaxLength(500, ErrorMessage = "Description must be 500 characters or less.")]
        public string? Description { get; set; } 

        [Required]
        public DateTime? StartAT { get; set; }
        public DateTime? FinishAT { get; set; }
        public bool StillWorking { get; set; }

    }
}
