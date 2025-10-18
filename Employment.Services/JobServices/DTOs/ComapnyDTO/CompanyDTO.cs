using Employment.Infrastructure.Entitys;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.ComapnyDTO
{
    public class CompanyDTO
    {
      
        [MaxLength(50, ErrorMessage = " name must be 50 characters or less.")]
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$", ErrorMessage = "Name must contain letters and spaces only.")]
        public string? Name { get; set; }

        [MaxLength(50, ErrorMessage = "Address must be 50 characters or less.")]
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$", ErrorMessage = "Address must contain letters and spaces only.")]
        public string? Address { get; set; }

        [MaxLength(500, ErrorMessage = "About must be 500 characters or less.")]
        [RegularExpression(@"^[a-zA-Z\u0600-\u06FF\s]+$", ErrorMessage = "About must contain letters and spaces only.")]
        public string? About { get; set; }
        public string? ImagePath { get; set; }



    }
}
