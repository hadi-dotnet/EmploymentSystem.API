using Job.Services.JobServices.DTOs.JobDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;

namespace Job.API.Controllers
{
    [Route("api/Job")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost("AddJob")]
        public async Task<IActionResult> AddJob ([FromBody] JobDTO jobDTO)
        {
            var res = await _jobService.AddJob(jobDTO);
            if(!res.Success)
            {
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            }
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
        [HttpPut("UpdateJob")]
        public async Task<IActionResult> UpdateJob([FromBody]UpdateJobDTO jobDTO)
        {
            var res = await _jobService.UpdateJob(jobDTO);
            if (!res.Success)
            {
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            }
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
        [HttpDelete("DeleteJob")]
        public async Task<IActionResult> DeleteJob([FromQuery] int JobID)
        {
            var res = await _jobService.DeleteJob(JobID);
            if(!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
        [HttpPost("ApplyJob")]
        public async Task<IActionResult> ApplyJob([FromQuery] int JobID)
        {
            var res = await _jobService.ApplyJob(JobID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
        [HttpGet("GetJobs")]
        public async Task<IActionResult> GetJobs ([FromQuery] int PageNumber, [FromQuery] int PageSize)
        {
            var res =await _jobService.GetJobs(PageNumber, PageSize);
            if (!res.Success)
                return BadRequest(ApiResponse<PageResult<GetJobsDTO>>.ErrorResponse(res.Message));
            return Ok(ApiResponse<PageResult<GetJobsDTO>>.SuccessResponse( res.Data,res.Message));

        }
        [HttpGet("GetJobBySkillType")]
        public async Task<IActionResult> GetJobBySkillType([FromQuery] int PageNumber, [FromQuery] int PageSize)
        {
            var res = await _jobService.GetJobBySkillType(PageNumber, PageSize);
            if (!res.Success)
                return BadRequest(ApiResponse<PageResult<GetJobsDTO>>.ErrorResponse(res?.Message));
            return Ok(ApiResponse<PageResult<GetJobsDTO>>.SuccessResponse(res.Data,res.Message));
        }

        [HttpGet("GetApplyJobs")]
        public async Task<IActionResult> GetApplyJobs([FromQuery] int PageNumber, [FromQuery] int PageSize)
        {
            var res = await _jobService.GetApplyJobs(PageNumber, PageSize);
            if (!res.Success)
                return BadRequest(ApiResponse<PageResult<GetApplyJobDto>>.ErrorResponse(res?.Message));
            return Ok(ApiResponse<PageResult<GetApplyJobDto>>.SuccessResponse(res.Data,res?.Message));
        }
    }
}
