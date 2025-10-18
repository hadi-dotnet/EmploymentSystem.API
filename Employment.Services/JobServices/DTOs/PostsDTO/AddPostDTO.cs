using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.PostsDTO
{
    public class AddPostDTO
    {
        [MaxLength(600, ErrorMessage = "Content must be 500 characters or less.")]
        public string? Content { get; set; }
        public IFormFile? Image { get; set; }
       
    }
}
