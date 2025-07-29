using Employment.Infrastructure.Context;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.Business
{
    public class CompanyService : ICompanyService
    {
        private readonly AppDBContext _context;
        private readonly IUserService _userservice;
        private readonly IManageImageService _imageservice;
       


        public CompanyService(AppDBContext context, IUserService userservice, IManageImageService imageservice)
        {
            _context = context;
            _userservice = userservice;
            _imageservice = imageservice;
            
        }

        private string? SetORNull(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }
        

        public async Task<string?> SetImage(IFormFile Image)
        {
            var UserID = _userservice.GetCuurentUserID();
            _imageservice.DeleteOldImage(UserID);

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



        public async Task<Result> UpdateCompany(CompanyDTO companyDTO)
        {
            var UserID = _userservice.GetCuurentUserID();
            var Company =await _context.Companies.FirstOrDefaultAsync(x => x.UserID == UserID);
            if (Company == null)
            {
                return new Result { Success = false, Message = "Not Found" };
            }

            Company.Name = SetORNull(companyDTO.Name);
            Company.Address = SetORNull(companyDTO.Address);
            Company.About = SetORNull(companyDTO.About);
            Company.Image = SetORNull(companyDTO.ImagePath);

            await _context.SaveChangesAsync();
            return new Result { Success = true, Message = "Update Complete" };


        }


        public async Task<CompanyDTO?> GetCompanyInformation()
        {
            var UserID = _userservice.GetCuurentUserID();
            var Company = await _context.Companies.FirstOrDefaultAsync(x=>x.UserID == UserID);
            if (Company == null)
                return null;

            var companyDTO = new CompanyDTO
            {
                Name = SetORNull(Company.Name),
                Address = SetORNull(Company.Address),
                ImagePath = SetORNull(Company.Image),
                About = SetORNull(Company.About),
            };

            return companyDTO;


        }

        public async Task<List< FindCompanyDTO>?> FindCompany(string CompanyName)
        {
            if (string.IsNullOrWhiteSpace(CompanyName))
                return null;

            var CompanyList = await _context.Companies.Where(x =>!string.IsNullOrEmpty(x.Name)&& x.Name.ToLower().Contains(CompanyName))
                                .Select(x=> new FindCompanyDTO
                                {
                                    ComId = x.UserID,
                                    CompanyName = x.Name
                                }).ToListAsync();
            return CompanyList;

        }

       
    }
}
