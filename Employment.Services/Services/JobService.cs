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

namespace Job.Services.Services
{
    public class JobService : IJobService
    {
        private readonly AppDBContext _dbContext;
        private readonly IUserService _userService;

        public JobService(AppDBContext context, IUserService userService)
        {
            _dbContext = context;
            _userService = userService;
        }


        public async Task<Result> AddJob(JobDTO jobDTO)
        {
            if (_userService.GetRole() != UserTypeEnum.Company.ToString())
                return Result.Fail("You Have No Access");

            var companyId = _userService.GetCuurentUserID();
            var jobType = await _dbContext.SkillsType.FirstOrDefaultAsync(x => x.id == jobDTO.SkillTypeID);
            if (jobType == null)
                return Result.Fail($"Invalid Job Type ID: {jobDTO.SkillTypeID}");

            var jobEntity = new Jobs
            {
                Title = jobDTO.Title,
                SkillsTypeID = jobDTO.SkillTypeID,
                CompanyID = companyId,
                Content = jobDTO.Content,
                FullTimeORPartTime = jobDTO.FullTimeORPartTime,
                RemoteOROnSite = jobDTO.RemoteOROnSite,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            await _dbContext.Jobs.AddAsync(jobEntity);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Job Added Successfully");
        }

        public async Task<Result> UpdateJob(UpdateJobDTO jobDTO)
        {
            if (_userService.GetRole() != UserTypeEnum.Company.ToString())
                return Result.Fail("You Have No Access");

            var companyId = _userService.GetCuurentUserID();
            var job = await _dbContext.Jobs.FirstOrDefaultAsync(x => x.ID == jobDTO.JobID && x.CompanyID == companyId);
            if (job == null)
                return Result.Fail("Job Not Found or You Have No Access");

            job.Title = jobDTO.Title;
            job.RemoteOROnSite = jobDTO.RemoteOROnSite;
            job.Content = jobDTO.Content;
            job.SkillsTypeID = jobDTO.JobTypeID;
            job.FullTimeORPartTime = jobDTO.FullTimeORPartTime;
            job.IsActive = jobDTO.IsActive;

            await _dbContext.SaveChangesAsync();
            return Result.SuccessResult("Job Updated Successfully");
        }

        public async Task<Result> DeleteJob(int jobID)
        {
            if (_userService.GetRole() != UserTypeEnum.Company.ToString())
                return Result.Fail("You Have No Access");

            var companyId = _userService.GetCuurentUserID();
            var job = await _dbContext.Jobs.FirstOrDefaultAsync(x => x.ID == jobID && x.CompanyID == companyId);
            if (job == null)
                return Result.Fail("Job Not Found or You Have No Access");

            _dbContext.Jobs.Remove(job);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Job Deleted Successfully");
        }

        public async Task<Result> ApplyJob(int jobID)
        {
            if (_userService.GetRole() != UserTypeEnum.Employee.ToString())
                return Result.Fail("You Have No Access");

            var employeeId = _userService.GetCuurentUserID();
            var job = await _dbContext.Jobs.FirstOrDefaultAsync(x => x.ID == jobID);
            if (job == null)
                return Result.Fail("Job Not Found");

            var alreadyApplied = await _dbContext.ApplyJob.FirstOrDefaultAsync(x => x.EmployeeID == employeeId && x.JobID == jobID);
            if (alreadyApplied != null)
                return Result.Fail("You Already Applied");

            var applyJob = new ApplyJob { EmployeeID = employeeId, JobID = jobID };
            await _dbContext.ApplyJob.AddAsync(applyJob);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Applied Successfully");
        }

        public async Task<Result<PageResult<GetJobsDTO>?>> GetJobs(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return Result<PageResult<GetJobsDTO>?>.Fail("Invalid Page Parameters");

            var query = _dbContext.Jobs
                .Include(x => x.SkillsType)
                .Include(x => x.Company)
                .Where(x => x.IsActive)
                .OrderByDescending(x => x.CreatedAt);

            var totalCount = await query.CountAsync();
            var jobs = await query.Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .Select(x => new GetJobsDTO
                                  {
                                      JobID = x.ID,
                                      CompanyName = x.Company.Name,
                                      CompanyID = x.CompanyID,
                                      Content = x.Content,
                                      CreatedAt = x.CreatedAt,
                                      FullTimeORPartTime = x.FullTimeORPartTime,
                                      RemoteOROnSite = x.RemoteOROnSite,
                                      IsActive = x.IsActive,
                                      SkillTypeName = x.SkillsType.TypeName,
                                      Title = x.Title
                                  }).ToListAsync();

            return Result<PageResult<GetJobsDTO>?>.SuccessResult(new PageResult<GetJobsDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = jobs
            }, "Success");
        }

        public async Task<Result<PageResult<GetJobsDTO>?>> GetJobBySkillType(int pageNumber, int pageSize)
        {
            if (_userService.GetRole() != UserTypeEnum.Employee.ToString())
                return Result<PageResult<GetJobsDTO>?>.Fail("You Have No Access");

            var employeeId = _userService.GetCuurentUserID();
            var jobs = await (from em in _dbContext.Employees
                              join sk in _dbContext.Skills on em.UserID equals sk.EmployeeID
                              join jb in _dbContext.Jobs on sk.SkillTypeID equals jb.SkillsTypeID
                              where em.UserID == employeeId
                              select new GetJobsDTO
                              {
                                  JobID = jb.ID,
                                  Content = jb.Content,
                                  CreatedAt = jb.CreatedAt,
                                  FullTimeORPartTime = jb.FullTimeORPartTime,
                                  IsActive = jb.IsActive,
                                  RemoteOROnSite = jb.RemoteOROnSite,
                                  SkillTypeName = jb.SkillsType.TypeName,
                                  Title = jb.Title
                              })
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .OrderByDescending(x => x.CreatedAt)
                              .ToListAsync();

            return Result<PageResult<GetJobsDTO>?>.SuccessResult(new PageResult<GetJobsDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = jobs.Count,
                Items = jobs
            }, "Success");
        }

        public async Task<Result<PageResult<GetApplyJobDto>?>> GetApplyJobs(int pageNumber, int pageSize)
        {
            if (_userService.GetRole() != UserTypeEnum.Company.ToString())
                return Result<PageResult<GetApplyJobDto>?>.Fail("You Have No Access");

            var companyId = _userService.GetCuurentUserID();
            var applyJobs = await (from cm in _dbContext.Companies
                                   join jb in _dbContext.Jobs on cm.UserID equals jb.CompanyID
                                   join ap in _dbContext.ApplyJob on jb.ID equals ap.JobID
                                   where cm.UserID == companyId
                                   select new GetApplyJobDto
                                   {
                                       JobID = jb.ID,
                                       EmployeeID = ap.EmployeeID,
                                       FullName = ap.Employee.FirstName + " " + ap.Employee.secoundName + " " + ap.Employee.LastName
                                   })
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return Result<PageResult<GetApplyJobDto>?>.SuccessResult(new PageResult<GetApplyJobDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = applyJobs.Count,
                Items = applyJobs
            }, "Success");
        }
    }

}
