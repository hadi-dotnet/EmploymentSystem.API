using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Job.API.Dtos
{
    public class UpdateCompanyDTO
    {
        [Required]
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? About { get; set; }
        public IFormFile? Logo { get; set; }
    }
}
