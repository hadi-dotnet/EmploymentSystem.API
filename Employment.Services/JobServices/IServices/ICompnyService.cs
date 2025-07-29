using Job.Services.JobServices.DTOs.ComapnyDTO;
using Job.Services.JobServices.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.Services
{
    public interface ICompanyService
    {
        Task<Result> UpdateCompany (CompanyDTO companyDTO);

        Task<CompanyDTO?> GetCompanyInformation();
        Task<List<FindCompanyDTO>?> FindCompany(string CompanyName);
        Task<string?> SetImage(IFormFile Image);

    }
}
