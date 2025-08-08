using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.EmployeeDTO;
using Job.Services.JobServices.DTOs.SkillsDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.Business
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDBContext _dbContext;

        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;


        public EmployeeService(AppDBContext appdbcontex, IUserService user, IMemoryCache memory)
        {
            _dbContext = appdbcontex;
            _userService = user;
            _cache = memory;

        }

        private string? SetORNull(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private async Task<List<SkillsType>> GetSkillsChach()
        {
            return await _cache.GetOrCreateAsync("AllSkills", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                return await _dbContext.SkillsType.ToListAsync();
            });
        }

        public async Task<Result> UpdateEmployee(EmployeeDTO updateEmployeeDTO)
        {
            var UserRole = _userService.GetRole();
            if (UserRole != UserTypeEnum.Employee.ToString())
                return Result.Fail("User Type Error .. You Have No access");

            var UserID = _userService.GetCuurentUserID();
            var employee = await _dbContext.Employees.FirstOrDefaultAsync(x => x.UserID == UserID);
            if (employee == null)
                return Result.Fail("User Not Found");


            employee.FirstName = SetORNull(updateEmployeeDTO.FirstName);
            employee.secoundName = SetORNull(updateEmployeeDTO.secoundName);
            employee.LastName = SetORNull(updateEmployeeDTO.LastName);
            employee.Address = SetORNull(updateEmployeeDTO.Address);
            employee.AboutYou = SetORNull(updateEmployeeDTO.AboutYou);
            employee.UniverCity = SetORNull(updateEmployeeDTO.UniverCity);

            await _dbContext.SaveChangesAsync();
            return Result.SuccessResult("Updated Success");
        }

        public async Task<Result< List< FindSkillsDTO>>> FindSkills(string SkillName)
        {
            var SkillListChach =await GetSkillsChach();        
            var list = SkillListChach.Where(x => !string.IsNullOrEmpty(x.TypeName) &&
            x.TypeName.ToLower().Contains(SkillName)).Select(x => new FindSkillsDTO
            {
                ID = x.id,
                SkillName = x.TypeName
            }).ToList() ;

            return Result<List<FindSkillsDTO>>.SuccessResult(list, "Found");
        }

        public async Task<Result< GetInforamtionDTO?>?> GetEmployeeInformation()
        {
            var UserID = _userService.GetCuurentUserID();
            var em = new GetInforamtionDTO();
            var Employee = await _dbContext.Employees.Include(x => x.Skills).ThenInclude(x => x.SkillsType)
                .Include(x => x.Skills).ThenInclude(x => x.ApplySkill).Include(x=>x.Experiences).
                FirstOrDefaultAsync(x => x.UserID == UserID);
            if (Employee == null)
                return Result<GetInforamtionDTO?>.Fail("Employee Not found");

            em.FirstName = Employee.FirstName;
            em.secoundName = Employee.secoundName;
            em.LastName = Employee.LastName;
            em.AboutYou = Employee.AboutYou;
            em.Address = Employee.Address;
            em.UniverCity = Employee.UniverCity;
            em.Skills = Employee.Skills.Select(skill => new SkillDTO
            {
                SkillID = skill.Id,
                SkillTypeName = skill.SkillsType?.TypeName,
                UserApplyed = skill.ApplySkill.Select(a => a.UserID).ToList()
            }).ToList();

            em.Experinces = Employee.Experiences.Select(x => new GetExperinceDTO
            {
                CompanyID = x.CompanyID,
                CompanyName = x.CompanyName,
                Description = x.Description,
                ID = x.ID,
                FinishAT = x.FinishAT,
                StartAT = x.StartAT,
                Title = x.Title,
            }).ToList();

            return Result<GetInforamtionDTO?>.SuccessResult(em,"Success");
        }

        public async Task<Result> AddSkill(List< AddSkillDTO> SkillTypeIDDTO)
        {
            var UserRole = _userService.GetRole();
            if (UserRole != UserTypeEnum.Employee.ToString())
                return Result.Fail("User Type Error .. You Have No access");

            var EmployeeID = _userService.GetCuurentUserID();
            var SkillList =await GetSkillsChach();
            foreach ( var skill in SkillTypeIDDTO )
            {
                var IsSkillExest = SkillList.FirstOrDefault(x=>x.id == skill.SkillTypeID);
                if (IsSkillExest == null)
                    return Result.Fail("Bad Requst .. The Skill Is Not Exest");
                
                var IsAlreadyExest = await _dbContext.Skills.FirstOrDefaultAsync(x=>x.SkillTypeID == skill.SkillTypeID&&x.EmployeeID==EmployeeID);
                if (IsAlreadyExest != null)
                    return Result.Fail("Bad Requst .. The Skill Already Exest");

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
            var UserID = _userService.GetCuurentUserID();
            if (SkillID <= 0)
                return Result.Fail("Invaled SkillID");

            var Em =await _dbContext.Skills.FirstOrDefaultAsync(x=>x.Id==SkillID);
            if (Em == null)
                return Result.Fail("The Skill Is Not Exest");

            if (Em.EmployeeID == UserID)
                return Result.Fail("You Cant Apply For Yourself");

            var ItemOfAplly = new ApplySkill();
            ItemOfAplly.SkillID = SkillID;
            ItemOfAplly.UserID= UserID;
            await _dbContext.ApplySkill.AddAsync(ItemOfAplly);
            await _dbContext.SaveChangesAsync();
            return Result.SuccessResult("Aplly Success");
        }

        public async Task<Result> UpdateSkill(int SkillID,string Name)
        {
            var UserRole = _userService.GetRole();
            if (UserRole != UserTypeEnum.Employee.ToString())
                return Result.Fail("User Type Error .. You Have No access");

            var UserID = _userService.GetCuurentUserID();
            if (SkillID <= 0 || string.IsNullOrEmpty(UserID) || string.IsNullOrWhiteSpace(Name))
                return Result.Fail("Invaled Input");

            var Skill =await _dbContext.Skills.FirstOrDefaultAsync(x=>x.Id == SkillID);
            if (Skill == null)
                return Result.Fail("Not Found");

            if (Skill.EmployeeID != UserID)
                return Result.Fail("You Have No Access");
                   
            await _dbContext.SaveChangesAsync();
            return Result.SuccessResult("updated complete");

        }

        public async Task<Result> DeleteSkill(int SkillID)
        {
            var UserRole = _userService.GetRole();
            if (UserRole != UserTypeEnum.Employee.ToString())
                return Result.Fail("User Type Error .. You Have No access");

            var UserID = _userService.GetCuurentUserID();
            if (SkillID <= 0 || string.IsNullOrEmpty(UserID))
                return Result.Fail("Invaled Input");
          

            var Skill = await _dbContext.Skills.FirstOrDefaultAsync(x=> x.Id == SkillID);
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
