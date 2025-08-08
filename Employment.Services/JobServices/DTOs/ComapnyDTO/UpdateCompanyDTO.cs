using Microsoft.AspNetCore.Http;

namespace Job.API.Dtos
{
    public class UpdateCompanyDTO
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? About { get; set; }
        public IFormFile? Logo { get; set; }
    }
}
