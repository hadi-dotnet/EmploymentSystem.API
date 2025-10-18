using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.JobDTO;
using Job.Services.JobServices.DTOs.SkillsDTO;
using Job.Services.JobServices.IServices;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
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
        private readonly IcacheService _cache;

        public JobService(AppDBContext context, IUserService userService,IcacheService cache)
        {
            _dbContext = context;
            _userService = userService;
            _cache = cache;
        }

        private async Task<List<SkillsType>> GetAllSkillsType()
        {
            return await _cache.GetSkillsAsync(async () =>
            {
                return await _dbContext.SkillsType.ToListAsync();
            });
        }

        public async Task<Result> AddJob(JobDTO jobDTO, EnumFullTimeORPartTime FullTimeORPartTime, EnumRemoteOROnSite RemoteOROnSite)
        {
            if (_userService.GetRole() != UserTypeEnum.Company.ToString())
                return Result.Fail("You Have No Access");

            if (string.IsNullOrWhiteSpace(jobDTO.Title) || string.IsNullOrWhiteSpace(jobDTO.Content))
                return Result.Fail("Invaled Title or Content");

            if (jobDTO.SkillTypeID == null || jobDTO.SkillTypeID.Count == 0)
                return Result.Fail("You Have To Enter Skills");

            if (!Enum.IsDefined(typeof(EnumFullTimeORPartTime),FullTimeORPartTime))
                return Result.Fail("Invaled FullTime or PartTime Input");

            if (!Enum.IsDefined(typeof(EnumRemoteOROnSite),RemoteOROnSite))
                return Result.Fail("Invaled Remote or OnSite Input");

            var companyId = _userService.GetCurrentUserID();
            if (companyId == null)
                return Result.Fail("Token Error");

            var Name = await _dbContext.Companies.AsNoTracking().Where(x => x.UserID == companyId).Select(x => x.Name).FirstOrDefaultAsync();
            if (Name == null)
                return Result.Fail("You Have To Enter Your Name");

            var SKillList = await GetAllSkillsType();
            var validSkillIds = SKillList.Select(x => x.id).ToHashSet();
            foreach (var item in jobDTO.SkillTypeID)
            {
                var res = validSkillIds.Contains(item);
                if (!res)
                    return Result.Fail($"The Skill with id = {item} Is Not Exest");                
            }

            using (var Transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var Job = new Jobs
                    {
                        Title = jobDTO.Title,
                        CompanyID = companyId,
                        Content = jobDTO.Content,
                        FullTimeORPartTime = FullTimeORPartTime,
                        RemoteOROnSite = RemoteOROnSite,
                        IsActive = true,
                        CreatedAt = DateTime.Now

                    };

                    await _dbContext.Jobs.AddAsync(Job);
                    await _dbContext.SaveChangesAsync();

                    var Skills = jobDTO.SkillTypeID.Select(id => new JobSkillType
                    {
                        JobID = Job.ID,
                        SkillTypeID = id,
                    }).ToList();

                    await _dbContext.JobSkillType.AddRangeAsync(Skills);
                    await _dbContext.SaveChangesAsync();

                    await Transaction.CommitAsync();
                }
                catch (Exception e)
                {
                   await Transaction.RollbackAsync();
                    return Result.Fail(e.Message); 
                }
            }

            return Result.SuccessResult("Added Success");
        }

        public async Task<Result> UpdateJob(JobDTO jobDTO,int JobID, EnumFullTimeORPartTime FullTimeORPartTime, EnumRemoteOROnSite RemoteOROnSite,bool IsActive)
        {
            if (_userService.GetRole() != UserTypeEnum.Company.ToString())
                return Result.Fail("You Have No Access");

            if (string.IsNullOrWhiteSpace(jobDTO.Title) || string.IsNullOrWhiteSpace(jobDTO.Content))
                return Result.Fail("Invaled Title or Content");

            if (jobDTO.SkillTypeID == null || jobDTO.SkillTypeID.Count == 0)
                return Result.Fail("You Have To Enter Skills");

            if (!Enum.IsDefined(typeof(EnumFullTimeORPartTime), FullTimeORPartTime))
                return Result.Fail("Invaled FullTime or PartTime Input");

            if (!Enum.IsDefined(typeof(EnumRemoteOROnSite), RemoteOROnSite))
                return Result.Fail("Invaled Remote or OnSite Input");


            var companyId = _userService.GetCurrentUserID();
            if (companyId == null)
                return Result.Fail("Token Error");

            var job = await _dbContext.Jobs.FirstOrDefaultAsync(x => x.ID == JobID);
            if (job == null)
                return Result.Fail("Not Found");

            if (job.CompanyID != companyId)
                return Result.Fail("You Have No Access");

            var SKillList = await GetAllSkillsType();
            var validSkillIds = SKillList.Select(x => x.id).ToHashSet();
            foreach (var item in jobDTO.SkillTypeID)
            {
                var res = validSkillIds.Contains(item);
                if (!res)
                    return Result.Fail($"The Skill with id = {item} Is Not Exest");
            }

            using(var Tranaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var GetSkills =await _dbContext.JobSkillType.Where(x => x.JobID == JobID).ToListAsync();
                    _dbContext.RemoveRange(GetSkills);

                    var Skills = jobDTO.SkillTypeID.Select(id => new JobSkillType
                    {
                        JobID = JobID,
                        SkillTypeID = id,
                    }).ToList();
                    await _dbContext.JobSkillType.AddRangeAsync(Skills);

                    job.Title = jobDTO.Title;
                    job.Content = jobDTO.Content;
                    job.FullTimeORPartTime = FullTimeORPartTime;
                    job.RemoteOROnSite = RemoteOROnSite;
                    job.IsActive = IsActive;

                    await _dbContext.SaveChangesAsync();
                    await Tranaction.CommitAsync();


                }
                catch (Exception e)
                {
                    await Tranaction.RollbackAsync();
                    return Result.Fail(e.Message);
                }
            }
           return Result.SuccessResult("Updated Success");

        }

        public async Task<Result> DeleteJob(int jobID)
        {
            if (_userService.GetRole() != UserTypeEnum.Company.ToString())
                return Result.Fail("You Have No Access");

            if (jobID < 0)
                return Result.Fail("Invaled Input");

            var companyId = _userService.GetCurrentUserID();
            if (companyId == null)
                return Result.Fail("Token Error");

            var job = await _dbContext.Jobs.FirstOrDefaultAsync(x => x.ID == jobID );
            if (job == null)
                return Result.Fail("Not Found");

            if (job.CompanyID != companyId)
                return Result.Fail("You Have No Access");

            using (var Transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var Skills = await _dbContext.JobSkillType.Where(x => x.JobID == jobID).ToListAsync();
                    _dbContext.JobSkillType.RemoveRange(Skills);
                    _dbContext.Jobs.Remove(job);

                    await _dbContext.SaveChangesAsync();
                    await Transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    await Transaction.RollbackAsync();
                    return Result.Fail(e.Message);
                }
            }
            return Result.SuccessResult("Deleted Success");
        }

        public async Task<Result> ApplyJob(int jobID)
        {
            if (_userService.GetRole() != UserTypeEnum.Employee.ToString())
                return Result.Fail("You Have No Access");

            if (jobID < 0)
                return Result.Fail("Invaled Input");

            var employeeId = _userService.GetCurrentUserID();
            if (employeeId == null)
                return Result.Fail("Token Error");

            var Name = await _dbContext.Employees.Where(x => x.UserID == employeeId).Select(x=>new
            {
                first = x.FirstName,
                secound = x.secoundName,
                last = x.LastName
            }).FirstOrDefaultAsync();
            if (Name == null|| Name.first == null|| Name.secound == null|| Name.last == null)
                return Result.Fail("You Have To Enter Your Name");

            var job = await _dbContext.Jobs.FirstOrDefaultAsync(x => x.ID == jobID);
            if (job == null)
                return Result.Fail("Not Found");

            var alreadyApplied = await _dbContext.ApplyJob.AnyAsync(x => x.EmployeeID == employeeId && x.JobID == jobID);
            if (alreadyApplied)
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


            var jobs = await _dbContext.Jobs
                            .Include(j => j.Company) 
                            .Include(j => j.JobSkillTypes) 
                            .ThenInclude(js => js.SkillsType) 
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .Select(j => new GetJobsDTO
                            {
                                JobID = j.ID,
                                CompanyName = j.Company.Name,
                                CompanyID = j.CompanyID,
                                Content = j.Content,
                                CreatedAt = j.CreatedAt,
                                FullTimeORPartTime = j.FullTimeORPartTime.ToString(),
                                RemoteOROnSite = j.RemoteOROnSite.ToString(),
                                IsActive = j.IsActive,
                                Title = j.Title,
                                SkillTypeName = j.JobSkillTypes.Select(js => new SkillsTypeDTO
                                {
                                    ID = js.SkillsType.id,
                                    SkillTypeName = js.SkillsType.TypeName
                                }).ToList()
                            })
                            .ToListAsync();


            if (!jobs.Any())
                return Result<PageResult<GetJobsDTO>?>.Fail("There Is No Jobs");

            return Result<PageResult<GetJobsDTO>?>.SuccessResult(new PageResult<GetJobsDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = jobs
            }, "Success");
           
        }

        public async Task<Result<PageResult<GetJobsDTO>?>> GetJobBySkillType(int pageNumber, int pageSize)
        {
            if (_userService.GetRole() != UserTypeEnum.Employee.ToString())
                return Result<PageResult<GetJobsDTO>?>.Fail("You Have No Access");

            var UserID = _userService.GetCurrentUserID();
            if (UserID == null)
                return Result<PageResult<GetJobsDTO>?>.Fail("Token Error");

            var SkillsID = await _dbContext.Skills.Where(x=>x.EmployeeID== UserID)
                                        .Select(x=>x.SkillTypeID).ToListAsync();

            var Jobs = await _dbContext.Jobs
             .Include(x => x.JobSkillTypes)
                 .ThenInclude(x => x.SkillsType)
             .Where(x => x.JobSkillTypes.Any(js => SkillsID.Contains(js.SkillTypeID)))
             .Skip((pageNumber - 1) * pageSize)
             .Take(pageSize).Select(j=>new GetJobsDTO
             {
                 JobID = j.ID,
                 CompanyName = j.Company.Name,
                 CompanyID = j.CompanyID,
                 Content = j.Content,
                 CreatedAt = j.CreatedAt,
                 FullTimeORPartTime = j.FullTimeORPartTime.ToString(),
                 RemoteOROnSite = j.RemoteOROnSite.ToString(),
                 IsActive = j.IsActive,
                 Title = j.Title,
                 SkillTypeName = j.JobSkillTypes.Select(js => new SkillsTypeDTO
                 {
                     ID = js.SkillsType.id,
                     SkillTypeName = js.SkillsType.TypeName
                 }).ToList()
             })
             .OrderByDescending(x => x.CreatedAt)
             .ToListAsync();

            if (!Jobs.Any())
                return Result<PageResult<GetJobsDTO>?>.Fail("Not Found");

            return Result<PageResult<GetJobsDTO>?>.SuccessResult(new PageResult<GetJobsDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = Jobs
            }, "Success");

           
        }

        public async Task<Result<PageResult<GetApplyJobDto>?>> GetApplyJobs(int pageNumber, int pageSize)
        {
            if (_userService.GetRole() != UserTypeEnum.Company.ToString())
                return Result<PageResult<GetApplyJobDto>?>.Fail("You Have No Access");

            var companyId = _userService.GetCurrentUserID();
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
                Items = applyJobs
            }, "Success");
        }
    }

}
