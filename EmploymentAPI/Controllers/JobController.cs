using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.JobDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;

namespace Job.API.Controllers
{
    [Authorize]
    [Route("api/Job")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost("Add-Job")]
        public async Task<IActionResult> AddJob ([FromBody] JobDTO Job,[FromQuery] EnumFullTimeORPartTime FullTimeORPartTime, [FromQuery] EnumRemoteOROnSite RemoteOROnSite)
        {
            var res = await _jobService.AddJob(Job,FullTimeORPartTime,RemoteOROnSite);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));       
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
        [HttpPut("Update-Job")]
        public async Task<IActionResult> UpdateJob([FromBody]JobDTO Job,[FromQuery]int JobID, [FromQuery] EnumFullTimeORPartTime FullTimeORPartTime, [FromQuery] EnumRemoteOROnSite RemoteOROnSite, [FromQuery] bool IsActive)
        {
            var res = await _jobService.UpdateJob(Job,JobID, FullTimeORPartTime, RemoteOROnSite,IsActive);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
        [HttpDelete("Delete-Job")]
        public async Task<IActionResult> DeleteJob([FromQuery] int JobID)
        {
            var res = await _jobService.DeleteJob(JobID);
            if(!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
        [HttpPost("Apply-Job")]
        public async Task<IActionResult> ApplyJob([FromQuery] int JobID)
        {
            var res = await _jobService.ApplyJob(JobID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
        [HttpGet("Get-Jobs")]
        public async Task<IActionResult> GetJobs ([FromQuery] int PageNumber, [FromQuery] int PageSize)
        {
            var res =await _jobService.GetJobs(PageNumber, PageSize);
            if (!res.Success)
                return BadRequest(ApiResponse<PageResult<GetJobsDTO>>.ErrorResponse(res.Message));
            return Ok(ApiResponse<PageResult<GetJobsDTO>>.SuccessResponse( res.Data,res.Message));

        }
        [HttpGet("Get-Job-By-Skill-Type")]
        public async Task<IActionResult> GetJobBySkillType([FromQuery] int PageNumber, [FromQuery] int PageSize)
        {
            var res = await _jobService.GetJobBySkillType(PageNumber, PageSize);
            if (!res.Success)
                return BadRequest(ApiResponse<PageResult<GetJobsDTO>>.ErrorResponse(res?.Message));
            return Ok(ApiResponse<PageResult<GetJobsDTO>>.SuccessResponse(res.Data,res.Message));
        }

        [HttpGet("Get-Apply-Jobs")]
        public async Task<IActionResult> GetApplyJobs([FromQuery] int PageNumber, [FromQuery] int PageSize)
        {
            var res = await _jobService.GetApplyJobs(PageNumber, PageSize);
            if (!res.Success)
                return BadRequest(ApiResponse<PageResult<GetApplyJobDto>>.ErrorResponse(res?.Message));
            return Ok(ApiResponse<PageResult<GetApplyJobDto>>.SuccessResponse(res.Data,res?.Message));
        }
    }
}
