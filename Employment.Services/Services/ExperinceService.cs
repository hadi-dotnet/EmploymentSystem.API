using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.EmployeeDTO;
using Job.Services.JobServices.DTOs.ExperinceDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.Business
{
    public class ExperinceService:IExperinceService
    {
        private readonly AppDBContext _dbContext;
        private readonly IUserService _userService;
        public ExperinceService(AppDBContext context, IUserService userService)
        {
            _dbContext = context;
            _userService = userService;
        }

        public async Task<Result> AddExperince(ExperinceDTO experienceDTO)
        {
            var UserRole = _userService.GetRole();
            if (UserRole != UserTypeEnum.Employee.ToString())
                return Result.Fail("User Type Error .. You Have No access");

            var CompanyID = "";
            var CompanyName = "";
            var EmID = _userService.GetCuurentUserID();
            var TryCopmany = await _dbContext.Companies.FirstOrDefaultAsync(x => x.UserID == experienceDTO.Company);

            if(TryCopmany == null)
            {
                CompanyName = experienceDTO.Company;
                CompanyID = null;
            }
            else
            {
                CompanyID = experienceDTO.Company;
                CompanyName = null;
            }
            
            DateTime? FinishAT = DateTime.Now;
            if (experienceDTO.StillWorking)
            {
                FinishAT = null;
            }
            else if (experienceDTO.StartAT < experienceDTO.FinishAT)
            {
                FinishAT = experienceDTO.FinishAT;

            }
            else
                return Result.Fail("Time error.. End time is less than the end time");

            var NewExperince = new Experience
            { 
                EmployeeID=EmID,
                CompanyID = CompanyID,
                CompanyName=CompanyName,
                Title = experienceDTO.Title,
                Description = experienceDTO.Description,
                StartAT = experienceDTO.StartAT,
                FinishAT = FinishAT,
            };

            await _dbContext.Experience.AddAsync(NewExperince);
            await _dbContext.SaveChangesAsync();
            return Result.SuccessResult("Added complate");
        }

        public async Task<Result> UpdateExperince(ExperinceDTO experienceDTO,int ExperinceID)
        {
            var UserRole = _userService.GetRole();
            if (UserRole != UserTypeEnum.Employee.ToString())
                return Result.Fail("User Type Error .. You Have No access");

            var EmployeeID = _userService.GetCuurentUserID();
            var ex = await _dbContext.Experience.FirstOrDefaultAsync(x=>x.ID == ExperinceID&&x.EmployeeID==EmployeeID);
            if (ex == null)
                return Result.Fail("You Have No access");

            var TryCopmany = await _dbContext.Companies.FirstOrDefaultAsync(x => x.UserID == experienceDTO.Company);
            if (TryCopmany == null)
            {
                ex.CompanyName = experienceDTO.Company;
                ex.CompanyID = null;
            }
            else
            {
                ex.CompanyID = experienceDTO.Company;
                ex.CompanyName = null;
            }

            DateTime? FinishAT = DateTime.Now;
            if (experienceDTO.StillWorking)
            {
                FinishAT = null;
            }
            else if (experienceDTO.StartAT < experienceDTO.FinishAT)
            {
                FinishAT = experienceDTO.FinishAT;

            }
            else
                return Result.Fail("Time error.. End time is less than the end time");

            ex.StartAT = experienceDTO.StartAT;
            ex.Description = experienceDTO.Description;
            ex.Title = experienceDTO.Title;
            ex.FinishAT = experienceDTO.FinishAT;

            await _dbContext.SaveChangesAsync();
            return Result.SuccessResult("Updated Success");
        }

        public async Task<Result> DeleteExperince(int ExperinceID)
        {
            var UserRole = _userService.GetRole();
            if (UserRole != UserTypeEnum.Employee.ToString())
                return Result.Fail("User Type Error .. You Have No access");

            var Emid = _userService.GetCuurentUserID();

            var Exper = await _dbContext.Experience.FirstOrDefaultAsync(x => x.ID == ExperinceID);
            if (Exper == null)
                return Result.Fail("Experience Not Found");

            if (Exper.EmployeeID != Emid)
                return Result.Fail("You Have No Access");

            _dbContext.Remove(Exper);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Deleted Complete");

        }

        public async Task<Result> ApplyExperince(int ExperinceID)
        {
            
            var UserRole = _userService.GetRole();
            if (UserRole != UserTypeEnum.Company.ToString())
                return Result.Fail("User Type Error .. You Have No access");

            var CompanyID = _userService.GetCuurentUserID();
            var res =await _dbContext.Experience.FirstOrDefaultAsync(x=>x.ID==ExperinceID);
            if (res == null)
                return Result.Fail("Experience Not Found");

            if (res.CompanyID != CompanyID)
                return Result.Fail("You Have No Access");

            var AddAplly = new ApplyExperience();
            AddAplly.ExperienceID = ExperinceID;

           await _dbContext.ApplyExperience.AddAsync(AddAplly);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Apply Compalete");


        }




    }
}
