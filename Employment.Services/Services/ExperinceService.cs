using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.ExperinceDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.Design;
using Job.Core.Helper;

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

            var EmployeeID = _userService.GetCurrentUserID();
            if (EmployeeID == null)
                return Result.Fail("Token Error");

            var Date = await _dbContext.Experience.Where(x => x.EmployeeID == EmployeeID).ToListAsync();

            if (experienceDTO.StillWorking && experienceDTO.StartAT <= Date.Max(x => x.StartAT))
                return Result.Fail("You've already worked this long!");
            else if (!experienceDTO.StillWorking&&(experienceDTO.StartAT > Date.Min(x => x.StartAT) && experienceDTO.FinishAT < Date.Max(x => x.FinishAT)))
                return Result.Fail("You've already worked this long!");
            else if (!experienceDTO.StillWorking && experienceDTO.StartAT > experienceDTO.FinishAT && experienceDTO.FinishAT > DateTime.Now)
                return Result.Fail("Invaled Time Input");

            var company = await _dbContext.Companies.FirstOrDefaultAsync(x => x.UserID == experienceDTO.CompanyNameorID);
            string? Companyname = "";
            string? CompanyID = "";
            if (company == null)
            {
                Companyname = experienceDTO.CompanyNameorID;
                CompanyID = null;
            }
            else
            {
                if (company.Name == null)
                    return Result.Fail("This company cannot be added because it does not have a name yet");
                Companyname = company.Name;
                CompanyID = company.UserID;
            }
                
  

            DateTime? finishAt = DateTime.Now;
            var newExperience = new Experience();

            if(experienceDTO.StillWorking)
            {
                var date = Date.Where(x => x.FinishAT == null).FirstOrDefault();
                if(date != null)
                {
                    date.FinishAT = DateTime.Now;
                    newExperience.StartAT =DateTime.Now;
                    newExperience.FinishAT = null;
                }
                else
                {
                    newExperience.StartAT = experienceDTO.StartAT;
                    newExperience.FinishAT = null;
                }
            }
            else
            {
                var date = Date.Where(x => x.FinishAT == null).FirstOrDefault();
                if (date != null)
                    date.FinishAT = DateTime.Now;
                newExperience.StartAT = experienceDTO.StartAT;
                newExperience.FinishAT = experienceDTO.FinishAT;
            }

            newExperience.EmployeeID = EmployeeID;
            newExperience.CompanyID = CompanyID;
            newExperience.CompanyName = Companyname;
            newExperience.Title = experienceDTO.Title;
            newExperience.Description = experienceDTO.Description;
           
            await _dbContext.Experience.AddAsync(newExperience);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Experience Added Successfully");
        }

        public async Task<Result> UpdateExperince(ExperinceDTO experienceDTO, int experienceId)
        {
            if (_userService.GetRole() != UserTypeEnum.Employee.ToString())
                return Result.Fail("You Have No Access");

            var EmployeeID = _userService.GetCurrentUserID();
            if (EmployeeID == null)
                return Result.Fail("Token Error");
            var experience = await _dbContext.Experience
                .FirstOrDefaultAsync(x => x.ID == experienceId );

            if (experience == null)
                return Result.Fail("Not Found");

            if(experience.EmployeeID != EmployeeID)
                return Result.Fail("You Have no Access");


            var Date = await _dbContext.Experience.Where(x => x.EmployeeID == EmployeeID).ToListAsync();

            if (experienceDTO.StillWorking && experienceDTO.StartAT <= Date.Max(x => x.StartAT))
                return Result.Fail("You've already worked this long!");
            else if (!experienceDTO.StillWorking && (experienceDTO.StartAT > Date.Min(x => x.StartAT) && experienceDTO.FinishAT < Date.Max(x => x.FinishAT)))
                return Result.Fail("You've already worked this long!");
            else if (!experienceDTO.StillWorking&&experienceDTO.StartAT > experienceDTO.FinishAT && experienceDTO.FinishAT > DateTime.Now)
                return Result.Fail("Invaled Time Input");

            var company = await _dbContext.Companies.FirstOrDefaultAsync(x => x.UserID == experienceDTO.CompanyNameorID);

            string? Companyname = "";
            string? CompanyID = "";
            if (company == null)
            {
                Companyname = experienceDTO.CompanyNameorID;
                CompanyID = null;
            }
            else
            {
                if (company.Name == null)
                    return Result.Fail("This company cannot be added because it does not have a name yet");
                Companyname = company.Name;
                CompanyID = company.UserID;
            }

            DateTime? finishAt = DateTime.Now;
            

            if (experienceDTO.StillWorking)
            {
                var date = Date.Where(x => x.FinishAT == null).FirstOrDefault();
                if (date != null)
                {
                    date.FinishAT = DateTime.Now;
                    experience.StartAT = DateTime.Now;
                    experience.FinishAT = null;
                }
                else
                {
                    experience.StartAT = experienceDTO.StartAT;
                    experience.FinishAT = null;
                }
            }
            else
            {
                var date = Date.Where(x => x.FinishAT == null).FirstOrDefault();
                if (date != null)
                    date.FinishAT = DateTime.Now;
                experience.StartAT = experienceDTO.StartAT;
                experience.FinishAT = experienceDTO.FinishAT;
            }

            experience.EmployeeID = EmployeeID;
            experience.CompanyID = CompanyID;
            experience.CompanyName = Companyname;
            experience.Title = experienceDTO.Title;
            experience.Description = experienceDTO.Description;

            
            await _dbContext.SaveChangesAsync();
            return Result.SuccessResult("Experience Updated Successfully");
        }

        public async Task<Result> DeleteExperince(int experienceId)
        {
            if (_userService.GetRole() != UserTypeEnum.Employee.ToString())
                return Result.Fail("You Have No Access");

            var employeeId = _userService.GetCurrentUserID();
            if (employeeId == null)
                return Result.Fail("Token Error");
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

            var companyId = _userService.GetCurrentUserID();
            if (companyId == null)
                return Result.Fail("Token Error");

            var User = await _dbContext.AppUser.Include(x => x.Company).Include(x => x.Employees).FirstOrDefaultAsync(x => x.Id == companyId);
            if (User == null)
                return Result.Fail("Error Tokent");

            if (!NameHelper.IsNameExist(User))
                return Result.Fail("You Have To Enter Your Name");

            var Experience = await _dbContext.Experience.FirstOrDefaultAsync(x => x.ID == experienceId);
            if (Experience == null)
                return Result.Fail("Experience Not Found");

            if (Experience.CompanyID != companyId)
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
