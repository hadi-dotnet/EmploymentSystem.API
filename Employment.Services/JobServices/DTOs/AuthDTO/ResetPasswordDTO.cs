using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.AuthDTO
{
    public class ResetPasswordDTO
    {
        [Required]
        public string? UserID { get; set; }
        [Required]

        public string? token { get; set; }
        [Required]

        public string? NewPassword { get; set; }


    }
}
