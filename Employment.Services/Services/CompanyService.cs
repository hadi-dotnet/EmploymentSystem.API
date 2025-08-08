using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.API.Dtos;
using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.ComapnyDTO;
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
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.Business
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDBContext _context;
        private readonly IUserService _userservice;
        
       
        public CompanyService(AppDBContext context, IUserService userservice)
        {
            _context = context;
            _userservice = userservice;
            
            
        }

        public void DeleteOldImage(string? ImagePath)
        {
           
            if (!string.IsNullOrEmpty(ImagePath) && System.IO.File.Exists(ImagePath))
            {
                System.IO.File.Delete(ImagePath);
            }

        }

        private string? SetORNull(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }
        
        public async Task<string?> SetImage(string? ImagePath,IFormFile? Image)
        {
            DeleteOldImage(ImagePath);

            if (Image == null || Image.Length == 0)
            {
                return null;
            }
            else
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(Image.FileName)}";
                var path = Path.Combine("C:\\ImageForJobProject\\", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }

                var logoPath = $"C:\\ImageForJobProject\\{fileName}";
                return logoPath;


            }
        }

        public async Task<Result> UpdateCompany(UpdateCompanyDTO CompanyDTO)
        {
            var Role = _userservice.GetRole();
            if (Role != UserTypeEnum.Company.ToString())
                return Result.Fail("You Have No Access");

            var UserID = _userservice.GetCuurentUserID();
            var Company = await _context.Companies.FirstOrDefaultAsync(x => x.UserID == UserID);
            if (Company == null)
            {
                return Result.Fail("Not Found");
            }

            Company.Image = await SetImage(Company.Image,CompanyDTO.Logo);
            Company.Name = SetORNull(CompanyDTO.Name);
            Company.Address = SetORNull(CompanyDTO.Address);
            Company.About = SetORNull(CompanyDTO.About);

            await _context.SaveChangesAsync();
            return Result.SuccessResult("Update Compalete");
        }

        public async Task<Result< CompanyDTO?>> GetCompanyInformation()
        {
            var UserID = _userservice.GetCuurentUserID();
            var Company = await _context.Companies.FirstOrDefaultAsync(x=>x.UserID == UserID);
            if (Company == null)
                return Result<CompanyDTO?>.Fail("Not Found");

            var companyDTO = new CompanyDTO
            {
                Name = SetORNull(Company.Name),
                Address = SetORNull(Company.Address),
                ImagePath = SetORNull(Company.Image),
                About = SetORNull(Company.About),
            };

            return Result<CompanyDTO?>.SuccessResult(companyDTO, "Success");
        }

        public async Task<Result<List<FindCompanyDTO>?>> FindCompany(string CompanyName)
        {
            if (string.IsNullOrWhiteSpace(CompanyName))
                return Result<List<FindCompanyDTO>?>.Fail("Not Found");

            var companies = await _context.Companies
                .Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(CompanyName.ToLower()))
                .Select(company => new FindCompanyDTO
                {
                    ComId = company.UserID,
                    CompanyName = company.Name
                })
                .ToListAsync();

            if (companies.Count == 0)
                return Result<List<FindCompanyDTO>?>.Fail("Not Found");

            return Result<List<FindCompanyDTO>?>.SuccessResult(companies, "Success");

        }

       
    }
}
