using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.JobDTO;
using Job.Services.JobServices.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.Services
{
    public interface IJobService
    {
        Task<Result> AddJob(JobDTO jobDTO, EnumFullTimeORPartTime FullTimeORPartTime, EnumRemoteOROnSite RemoteOROnSite);
        Task<Result> UpdateJob(JobDTO jobDTO, int JobID, EnumFullTimeORPartTime FullTimeORPartTime, EnumRemoteOROnSite RemoteOROnSite, bool IsActive);
        Task<Result> DeleteJob(int JobID);
        Task<Result> ApplyJob(int JobID);
        Task<Result<PageResult<GetJobsDTO>?>> GetJobs(int PageNumber, int PageSize);
        Task<Result<PageResult<GetJobsDTO>?>> GetJobBySkillType(int PageNumber, int PageSize);
        Task<Result<PageResult<GetApplyJobDto>?>> GetApplyJobs(int PageNumber, int PageSize);
    }
}
