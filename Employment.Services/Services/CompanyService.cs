using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.API.Dtos;
using Job.Core.Entitys;
using Job.Core.Extensions;
using Job.Services.JobServices.DTOs.ComapnyDTO;
using Job.Services.JobServices.IServices;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDBContext _context;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;

        public CompanyService(AppDBContext context, IUserService userService, IFileService fileService)
        {
            _context = context;
            _userService = userService;
            _fileService = fileService;
        }

        public async Task<Result> UpdateCompany(UpdateCompanyDTO companyDto)
        {
            if (_userService.GetRole() != UserTypeEnum.Company.ToString())
                return Result.Fail("You Have No Access");

            if (string.IsNullOrWhiteSpace(companyDto.Name))
                return Result.Fail("You Have To Enter Your Name");

            var userId = _userService.GetCurrentUserID();
            if (userId == null)
                return Result.Fail("Token Error");

            var company = await _context.Companies.FirstOrDefaultAsync(x => x.UserID == userId);

            if (company == null)
                return Result.Fail("Company Not Found");

            company.Image = await _fileService.SaveFileAsync(company.Image, companyDto.Logo);
            company.Name = companyDto.Name.SetOrNull();
            company.Address = companyDto.Address.SetOrNull();
            company.About = companyDto.About.SetOrNull();

            await _context.SaveChangesAsync();
            return Result.SuccessResult("Update Complete");
        }

        public async Task<Result<CompanyDTO?>> GetCompanyInformation()
        {
            var userID = _userService.GetCurrentUserID();
            if (userID == null)
                return Result<CompanyDTO?>.Fail("Token Error");
            var company = await _context.Companies.FirstOrDefaultAsync(x => x.UserID == userID);

            if (company == null)
                return Result<CompanyDTO?>.Fail("Not Found");

            var dto = new CompanyDTO
            {
                Name = company.Name.SetOrNull(),
                Address = company.Address.SetOrNull(),
                ImagePath = company.Image.SetOrNull(),
                About = company.About.SetOrNull()
            };

            return Result<CompanyDTO?>.SuccessResult(dto, "Success");
        }

        public async Task<Result<List<FindCompanyDTO>?>> FindCompany(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName))
                return Result<List<FindCompanyDTO>?>.Fail("Invaled Input");

            var companies = await _context.Companies
                .Where(x => !string.IsNullOrEmpty(x.Name) &&
                            x.Name.ToLower().Contains(companyName.ToLower()))
                .Select(c => new FindCompanyDTO
                {
                    ComId = c.UserID,
                    CompanyName = c.Name
                })
                .ToListAsync();

            if (!companies.Any())
                return Result<List<FindCompanyDTO>?>.Fail("Not Found");

            return Result<List<FindCompanyDTO>?>.SuccessResult(companies, "Success");

           
        }
    }


}
