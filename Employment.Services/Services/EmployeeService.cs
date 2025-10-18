using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.Core.Entitys;
using Job.Core.Helper;
using Job.Services.JobServices.DTOs.EmployeeDTO;
using Job.Services.JobServices.DTOs.SkillsDTO;
using Job.Services.JobServices.IServices;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDBContext _dbContext;
        private readonly IcacheService _cache;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        

        public EmployeeService(AppDBContext appdbcontex, IUserService user, IcacheService memory,IFileService fileService)
        {
            _dbContext = appdbcontex;
            _userService = user;
            _cache = memory;
            _fileService = fileService;

        }

        private string? SetORNull(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private async Task<List<SkillsType>> GetAllSkillsType ()
        {
            return await _cache.GetSkillsAsync(async () =>
            {
                return await _dbContext.SkillsType.ToListAsync();
            });
        }

        public async Task<Result> UpdateEmployee(EmployeeDTO updateEmployeeDTO)
        {
           
            if (_userService.GetRole() != UserTypeEnum.Employee.ToString())
                return Result.Fail("You Have No access");

            var UserID = _userService.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Token Error");

            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.UserID == UserID);
            if (employee == null)
                return Result.Fail("User Not Found");

            if (string.IsNullOrWhiteSpace(updateEmployeeDTO.FirstName) ||
               string.IsNullOrWhiteSpace(updateEmployeeDTO.secoundName) ||
               string.IsNullOrWhiteSpace(updateEmployeeDTO.LastName))
                return Result.Fail("First Name and Secound Name and Last Name in Required");


            employee.FirstName = SetORNull(updateEmployeeDTO.FirstName);
            employee.secoundName = SetORNull(updateEmployeeDTO.secoundName);
            employee.LastName = SetORNull(updateEmployeeDTO.LastName);
            employee.Address = SetORNull(updateEmployeeDTO.Address);
            employee.AboutYou = SetORNull(updateEmployeeDTO.AboutYou);
            employee.UniverCity = SetORNull(updateEmployeeDTO.UniverCity);
            employee.Image = await _fileService.SaveFileAsync(employee.Image, updateEmployeeDTO.Image);

            await _dbContext.SaveChangesAsync();
            return Result.SuccessResult("Updated Success");
        }

        public async Task<Result<List<FindSkillsDTO>>> FindSkills(string SkillName)
        {
            var SkillListChach = await GetAllSkillsType();
            var list = SkillListChach.Where(x => !string.IsNullOrEmpty(x.TypeName) &&
            x.TypeName.ToLower().Contains(SkillName)).Select(x => new FindSkillsDTO
            {
                ID = x.id,
                SkillName = x.TypeName
            }).ToList();

            return Result<List<FindSkillsDTO>>.SuccessResult(list, "Found");
        }

        public async Task<Result<GetInforamtionDTO?>?> GetEmployeeInformation()
        {
            var UserID = _userService.GetCurrentUserID();
            if (UserID == null)
                return Result<GetInforamtionDTO?>.Fail("Token Error");


            var em = new GetInforamtionDTO();
            var Employee = await _dbContext.Employees.Include(x => x.Skills).ThenInclude(x => x.SkillsType)
                .Include(x => x.Skills).ThenInclude(x => x.ApplySkill).Include(x => x.Experiences)
                .Where(x => x.UserID == UserID)
                .Select(em => new GetInforamtionDTO
                {
                    FirstName = em.FirstName,
                    secoundName = em.secoundName,
                    LastName = em.LastName,
                    AboutYou = em.AboutYou,
                    Address = em.Address,
                    UniverCity = em.UniverCity,
                    ImagePath = em.Image,
                    Skills = em.Skills.Select(sk => new SkillDTO
                    {
                        SkillID = sk.Id,
                        SkillTypeName = sk.SkillsType.TypeName,
                        UserApplyed = sk.ApplySkill.Select(a => a.AppUser.UserType == UserTypeEnum.Company?
                        a.AppUser.Company.Name:
                        $"{a.AppUser.Employees.FirstName} {a.AppUser.Employees.FirstName} {a.AppUser.Employees.FirstName}").ToList()
                    }).ToList(),
                    Experinces = em.Experiences.Select(x => new GetExperinceDTO
                    {
                        ID = x.ID,
                        CompanyID = x.CompanyID,
                        CompanyName = x.CompanyName,
                        Title = x.Title,
                        Description = x.Description,
                        FinishAT = x.FinishAT.ToString()??"Still Working",
                        StartAT = x.StartAT.ToString(),
                        IsApplyed = x.ApplyExperience.Any(a=>a.ExperienceID==x.ID)?"Yes":"NO"
                    }).ToList()

                }).
                FirstOrDefaultAsync();
            if (Employee == null)
                return Result<GetInforamtionDTO?>.Fail("Employee Not found");

            return Result<GetInforamtionDTO?>.SuccessResult(Employee, "Success");
        }

        public async Task<Result<GetInforamtionDTO?>?> GetEmployeeInformationByID(string EmployeeID)
        {
            var UserID = _userService.GetCurrentUserID();
            if (UserID == null)
                return Result<GetInforamtionDTO?>.Fail("Token Error");


            var em = new GetInforamtionDTO();
            var Employee = await _dbContext.Employees.Include(x => x.Skills).ThenInclude(x => x.SkillsType)
                .Include(x => x.Skills).ThenInclude(x => x.ApplySkill).Include(x => x.Experiences)
                .Where(x => x.UserID == EmployeeID)
                .Select(em => new GetInforamtionDTO
                {
                    FirstName = em.FirstName,
                    secoundName = em.secoundName,
                    LastName = em.LastName,
                    AboutYou = em.AboutYou,
                    Address = em.Address,
                    UniverCity = em.UniverCity,
                    ImagePath = em.Image,
                    Skills = em.Skills.Select(sk => new SkillDTO
                    {
                        SkillID = sk.Id,
                        SkillTypeName = sk.SkillsType.TypeName,
                        UserApplyed = sk.ApplySkill.Select(a => a.AppUser.UserType == UserTypeEnum.Company ?
                        a.AppUser.Company.Name :
                        $"{a.AppUser.Employees.FirstName} {a.AppUser.Employees.FirstName} {a.AppUser.Employees.FirstName}").ToList()
                    }).ToList(),
                    Experinces = em.Experiences.Select(x => new GetExperinceDTO
                    {
                        ID = x.ID,
                        CompanyID = x.CompanyID,
                        CompanyName = x.CompanyName,
                        Title = x.Title,
                        Description = x.Description,
                        FinishAT = x.FinishAT.ToString() ?? "Still Working",
                        StartAT = x.StartAT.ToString(),
                        IsApplyed = x.ApplyExperience.Any(a => a.ExperienceID == x.ID) ? "Yes" : "NO"
                    }).ToList()

                }).
                FirstOrDefaultAsync();
            if (Employee == null)
                return Result<GetInforamtionDTO?>.Fail("Employee Not found");

            return Result<GetInforamtionDTO?>.SuccessResult(Employee, "Success");
        }

        public async Task<Result> AddSkill(List<AddSkillDTO> SkillTypeIDDTO)
        {
            
            if (_userService.GetRole() != UserTypeEnum.Employee.ToString())
                return Result.Fail("You Have No access");

            var EmployeeID = _userService.GetCurrentUserID();
            if (EmployeeID == null)
                return Result.Fail("Token Error");

            var SkillList = await GetAllSkillsType();
            foreach (var skill in SkillTypeIDDTO)
            {
                var IsSkillExest = SkillList.FirstOrDefault(x => x.id == skill.SkillTypeID);
                if (IsSkillExest == null)
                    return Result.Fail("The Skill Is Not Exest");

                var IsAlreadyExest = await _dbContext.Skills.FirstOrDefaultAsync(x => x.SkillTypeID == skill.SkillTypeID && x.EmployeeID == EmployeeID);
                if (IsAlreadyExest != null)
                    return Result.Fail("The Skill Already Exest");

                var EnSkill = new Skills
                {
                    EmployeeID = EmployeeID,
                    SkillTypeID = skill.SkillTypeID,
                };

                await _dbContext.Skills.AddAsync(EnSkill);

            }
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Added success");
        }

        public async Task<Result> Applyskill(int SkillID)
        {
            var UserID = _userService.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Token Error");

            if (SkillID <= 0)
                return Result.Fail("Invaled SkillID");

            var User = await _dbContext.AppUser.Include(x => x.Company).Include(x => x.Employees).FirstOrDefaultAsync(x => x.Id == UserID);
            if (User == null)
                return Result.Fail("Error Tokent");

            if (!NameHelper.IsNameExist(User))
                return Result.Fail("You Have To Enter Your Name");

            var Em = await _dbContext.Skills.FirstOrDefaultAsync(x => x.Id == SkillID);
            if (Em == null)
                return Result.Fail("The Skill Is Not Exest");

            if (Em.EmployeeID == UserID)
                return Result.Fail("You Cant Apply For Yourself");

            var ItemOfAplly = new ApplySkill();
            ItemOfAplly.SkillID = SkillID;
            ItemOfAplly.UserID = UserID;
            await _dbContext.ApplySkill.AddAsync(ItemOfAplly);
            await _dbContext.SaveChangesAsync();
            return Result.SuccessResult("Apply Success");
        }


        public async Task<Result> DeleteSkill(int SkillID)
        {
            
            if (_userService.GetRole() != UserTypeEnum.Employee.ToString())
                return Result.Fail("You Have No access");

            var UserID = _userService.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Token Error");

            if (SkillID <= 0 || string.IsNullOrEmpty(UserID))
                return Result.Fail("Invaled Input");


            var Skill = await _dbContext.Skills.FirstOrDefaultAsync(x => x.Id == SkillID);
            if (Skill == null)
                return Result.Fail("not Found");

            if (Skill.EmployeeID != UserID)
                return Result.Fail("You Have No access");

            try
            {
                _dbContext.Skills.Remove(Skill);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {
                return Result.Fail("YOu Can not Delete Thes Skill");

            }
            return Result.SuccessResult("Deleted Success");

        }

    }
}
