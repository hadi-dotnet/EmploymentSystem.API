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

namespace Job.Services.Services
{
    public class ExperinceService : IExperinceService
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
            if (_userService.GetRole() != UserTypeEnum.Employee.ToString())
                return Result.Fail("You Have No Access");

            var employeeId = _userService.GetCuurentUserID();

            var company = await _dbContext.Companies.FirstOrDefaultAsync(x => x.UserID == experienceDTO.Company);

            var companyId = company != null ? experienceDTO.Company : null;
            var companyName = company == null ? experienceDTO.Company : null;


            DateTime? finishAt = DateTime.Now;
            if (experienceDTO.StillWorking)
            { finishAt = null; }
            else if (experienceDTO.StartAT < experienceDTO.FinishAT)
            { finishAt = experienceDTO.FinishAT; }
            else return Result.Fail("Time error.. End time is less than the end time");


            var newExperience = new Experience
            {
                EmployeeID = employeeId,
                CompanyID = companyId,
                CompanyName = companyName,
                Title = experienceDTO.Title,
                Description = experienceDTO.Description,
                StartAT = experienceDTO.StartAT,
                FinishAT = finishAt
            };

            await _dbContext.Experience.AddAsync(newExperience);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Experience Added Successfully");
        }

        public async Task<Result> UpdateExperince(ExperinceDTO experienceDTO, int experienceId)
        {
            if (_userService.GetRole() != UserTypeEnum.Employee.ToString())
                return Result.Fail("You Have No Access");

            var employeeId = _userService.GetCuurentUserID();
            var ex = await _dbContext.Experience
                .FirstOrDefaultAsync(x => x.ID == experienceId && x.EmployeeID == employeeId);

            if (ex == null)
                return Result.Fail("Experience Not Found or No Access");

            var company = await _dbContext.Companies.FirstOrDefaultAsync(x => x.UserID == experienceDTO.Company);

            ex.CompanyID = company != null ? experienceDTO.Company : null;
            ex.CompanyName = company == null ? experienceDTO.Company : null;


            DateTime? finishAt = DateTime.Now;
            if (experienceDTO.StillWorking)
            { finishAt = null; }
            else if (experienceDTO.StartAT < experienceDTO.FinishAT)
            { finishAt = experienceDTO.FinishAT; }
            else return Result.Fail("Time error.. End time is less than the end time");

            ex.Title = experienceDTO.Title;
            ex.Description = experienceDTO.Description;
            ex.StartAT = experienceDTO.StartAT;
            ex.FinishAT = finishAt;

            await _dbContext.SaveChangesAsync();
            return Result.SuccessResult("Experience Updated Successfully");
        }

        public async Task<Result> DeleteExperince(int experienceId)
        {
            if (_userService.GetRole() != UserTypeEnum.Employee.ToString())
                return Result.Fail("You Have No Access");

            var employeeId = _userService.GetCuurentUserID();
            var ex = await _dbContext.Experience.FirstOrDefaultAsync(x => x.ID == experienceId);

            if (ex == null)
                return Result.Fail("Experience Not Found");

            if (ex.EmployeeID != employeeId)
                return Result.Fail("You Have No Access");

            _dbContext.Experience.Remove(ex);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Experience Deleted Successfully");
        }

        public async Task<Result> ApplyExperince(int experienceId)
        {
            if (_userService.GetRole() != UserTypeEnum.Company.ToString())
                return Result.Fail("You Have No Access");

            var companyId = _userService.GetCuurentUserID();
            var ex = await _dbContext.Experience.FirstOrDefaultAsync(x => x.ID == experienceId);

            if (ex == null)
                return Result.Fail("Experience Not Found");

            if (ex.CompanyID != companyId)
                return Result.Fail("You Have No Access");

            var apply = new ApplyExperience
            {
                ExperienceID = experienceId
            };

            await _dbContext.ApplyExperience.AddAsync(apply);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Experience Applied Successfully");
        }
    }

}
