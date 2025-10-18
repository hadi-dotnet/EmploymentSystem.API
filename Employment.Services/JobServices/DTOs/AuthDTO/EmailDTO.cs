using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.AuthDTO
{
    public class EmailDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email! Must contain @ and domain after it")]
        public string? Email { get; set; }  
    }

    public class NewEmailDTO
    {
        [Required]
        [EmailAddress]
        public string? NewEmail { get; set; }
    }
}
