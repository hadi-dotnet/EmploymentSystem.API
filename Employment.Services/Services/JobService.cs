using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.JobDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Job.Services.Business
{
    public class JobService:IJobService
    {
        private readonly AppDBContext _dbContext;
        private readonly IUserService _userService;
        public JobService(AppDBContext context,IUserService userService)
        {
            _dbContext = context;
            _userService = userService;
        }

        public async Task<Result> AddJob(JobDTO jobDTO)
        {
            var UserRole = _userService.GetRole();
            if (UserRole != UserTypeEnum.Company.ToString())
                return new Result { Success = false, Message = "You Have No Access" };
            var CompanyID = _userService.GetCuurentUserID();

            var Jobtype =await _dbContext.SkillsType.FirstOrDefaultAsync(x=>x.id==jobDTO.SkillTypeID);
            if(Jobtype == null)
                return new Result { Success = false, Message = $"Bad Requst In Job Type .. Job Type = {jobDTO.SkillTypeID}" };



            var JobEntity = new Jobs
            {
                Title = jobDTO.Title,
                SkillsTypeID = jobDTO.SkillTypeID,
                CompanyID = CompanyID,
                Content = jobDTO.Content,
                FullTimeORPartTime = jobDTO.FullTimeORPartTime,
                RemoteOROnSite = jobDTO.RemoteOROnSite,
                IsActive = true,
                CreatedAt = DateTime.Now
                
                
                
            };

            await _dbContext.Jobs.AddAsync(JobEntity);
            await _dbContext.SaveChangesAsync();

            return new Result { Success = true, Message = "Added Complete" };

        }

        public async Task<Result> UpdateJob(UpdateJobDTO jobDTO)
        {
            var UserRole = _userService.GetRole();
            if (UserRole != UserTypeEnum.Company.ToString())
                return new Result { Success = false, Message = "You Have No Access" };
            var CompanyID = _userService.GetCuurentUserID();

            var Job = await _dbContext.Jobs.FirstOrDefaultAsync(x => x.ID == jobDTO.JobID && x.CompanyID == CompanyID);
            if (Job == null) return new Result { Success = false, Message = "You Have No Access" };

            Job.Title = jobDTO.Title;
            Job.RemoteOROnSite = jobDTO.RemoteOROnSite;
            Job.Content = jobDTO.Content;
            Job.SkillsTypeID = jobDTO.JobTypeID;
            Job.FullTimeORPartTime = jobDTO.FullTimeORPartTime;
            Job.IsActive = jobDTO.IsActive;
           

            await _dbContext.SaveChangesAsync();
            return new Result { Success = true, Message = "updated Complete" };

        }

        public async Task<Result> DeleteJob(int JobID)
        {
            var UserRole = _userService.GetRole();
            if (UserRole != UserTypeEnum.Company.ToString())
                return new Result { Success = false, Message = "You Have No Access" };
            var CompanyID = _userService.GetCuurentUserID();

            var Job =await _dbContext.Jobs.FirstOrDefaultAsync(x=>x.ID== JobID && x.CompanyID==CompanyID);
            if (Job == null) return new Result { Success = false, Message = "You Have No Access" };

            _dbContext.Jobs.Remove(Job);
            await _dbContext.SaveChangesAsync();
             return new Result { Success = true, Message = "Delete Complete" };


        }

        public async Task<Result> ApplyJob(int JobID)
        {
            var UserRole = _userService.GetRole();
            if (UserRole != UserTypeEnum.Employee.ToString())
                return new Result { Success = false, Message = "You Have No Access" };
            var EmployeeID = _userService.GetCuurentUserID();

            var IsJobExest =await _dbContext.Jobs.FirstOrDefaultAsync(x => x.ID == JobID);
            if (IsJobExest == null)
                return new Result { Success = false, Message = "Not Found" };

            var IsEmployeeApply = await _dbContext.ApplyJob.FirstOrDefaultAsync(x => x.EmployeeID == EmployeeID&&x.JobID==JobID);
            if (IsEmployeeApply != null)
                return new Result { Success = false, Message = "You Already Apply" };

            var ApllyJob = new ApplyJob { EmployeeID = EmployeeID,JobID = JobID };
            await _dbContext.ApplyJob.AddAsync(ApllyJob);
            await _dbContext.SaveChangesAsync();
                return new Result { Success=true,Message="Apply Complete"};


        }

        public async Task<JobResult<GetJobsDTO>?> GetJobs (int PageNumber, int PageSize)
        {
            if (PageNumber <= 0 || PageSize <= 0)
                return null;

            var Query = _dbContext.Jobs.Include(x=>x.SkillsType).Include(x=>x.Company).Where(x => x.IsActive == true).OrderByDescending(x=>x.CreatedAt);

            var TotalCount = await Query.CountAsync();

            var Jobs = await Query.Skip((PageNumber - 1) * PageSize).Take(PageSize).Select(x => new GetJobsDTO
            {
                CompanyNmae = x.Company.Name,
                CompanyID = x.CompanyID,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                FullTimeORPartTime = x.FullTimeORPartTime,
                RemoteOROnSite = x.RemoteOROnSite,
                IsActive = x.IsActive,
                SkillTypeName = x.SkillsType.TypeName,
                Title = x.Title,

            }).ToListAsync() ;

            var Result = new JobResult<GetJobsDTO>
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalCount = TotalCount,
                Items = Jobs,
            };
            return Result;



          
        }

        public async Task<JobResult<GetJobsDTO>?> GetJobBySkillType(int PageNumber, int PageSize)
        {
            var Role = _userService.GetRole();
            if (Role != UserTypeEnum.Employee.ToString())
                return null;
            var EmployeeID = _userService.GetCuurentUserID();

            var Jobs = await(from em in _dbContext.Employees 
                        join sk in _dbContext.Skills on em.UserID equals sk.EmployeeID
                        join jb in _dbContext.Jobs on sk.SkillTypeID equals jb.SkillsTypeID
                        where em.UserID ==EmployeeID
                        select new GetJobsDTO
                        {
                            Content = jb.Content,
                            CreatedAt = jb.CreatedAt,
                            FullTimeORPartTime= jb.FullTimeORPartTime,
                            IsActive= jb.IsActive,
                            RemoteOROnSite = jb.RemoteOROnSite,
                            SkillTypeName = jb.SkillsType.TypeName,
                            Title = jb.Title,
                        }).Skip((PageNumber - 1) * PageSize).Take(PageSize).OrderByDescending(x => x.CreatedAt).Distinct().ToListAsync();

            var TotalCount = Jobs.Count();

            var Result = new JobResult<GetJobsDTO>
            {
                PageNumber = PageNumber,
                PageSize = PageSize,
                TotalCount = TotalCount,
                Items = Jobs
            };
            return Result;

        }

        public async Task<JobResult<GetApplyJobDto>?> GetApplyJobs(int PageNumber, int PageSize)
        {
            var Role = _userService.GetRole();
            if (Role != UserTypeEnum.Company.ToString())
                return null;

            var CompanyID = _userService.GetCuurentUserID();

            var ApplyJobs =await (from cm in _dbContext.Companies
                         join jb in _dbContext.Jobs on cm.UserID equals jb.CompanyID
                         join ap in _dbContext.ApplyJob on jb.ID equals ap.JobID
                         where cm.UserID == CompanyID
                         select new GetApplyJobDto
                         {
                             EmployeeID = ap.EmployeeID,
                             FullName = ap.Employee.FirstName + " " + ap.Employee.secoundName + " " + ap.Employee.LastName

                         }).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();




            var res = new JobResult<GetApplyJobDto>
            {
                TotalCount = ApplyJobs.Count(),
                PageSize = PageSize,
                PageNumber = PageNumber,
                Items = ApplyJobs

            };
            return res;

        }

     





    }
}
