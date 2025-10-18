using Job.Core.Entitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.JobDTO
{
  
    public class JobDTO
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Title must be 50 characters or less.")]
        public string? Title { get; set; }

        [Required]
        [MaxLength(1500, ErrorMessage = "Content must be 1500 characters or less.")]
        public string? Content { get; set; }

        [Required]
        public HashSet<int>? SkillTypeID { get; set; }

    }
}
