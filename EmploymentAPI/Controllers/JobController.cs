using Job.Services.JobServices.DTOs.JobDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;

namespace Job.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpPost("AddJob")]
        public async Task<IActionResult> AddJob (JobDTO jobDTO)
        {
            var res = await _jobService.AddJob(jobDTO);
            if(!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Message);
        }

        [HttpPut("UpdateJob")]
        public async Task<IActionResult> UpdateJob(UpdateJobDTO jobDTO)
        {
            var res = await _jobService.UpdateJob(jobDTO);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }
            return Ok(res.Message);
        }

        [HttpDelete("DeleteJob")]
        public async Task<IActionResult> DeleteJob(int JobID)
        {
            var res = await _jobService.DeleteJob(JobID);
            if(!res.Success)
                return BadRequest(res.Message);
            return Ok(res.Message);
        }
        [HttpPost("ApplyJob")]
        public async Task<IActionResult> ApplyJob(int JobID)
        {
            var res = await _jobService.ApplyJob(JobID);
            if (!res.Success)
                return BadRequest(res.Message);
            return Ok(res.Message);
        }
        [HttpGet("GetJobs")]
        public async Task<IActionResult> GetJobs (int PageNumber,int PageSize)
        {
            var res =await _jobService.GetJobs(PageNumber, PageSize);
            if (res == null)
                return BadRequest();
            return Ok(res);

        }
        [HttpGet("GetJobBySkillType")]
        public async Task<IActionResult> GetJobBySkillType(int PageNumber, int PageSize)
        {
            var res = await _jobService.GetJobBySkillType(PageNumber, PageSize);
            if (res == null)
                return BadRequest();
            return Ok(res);
        }

        [HttpGet("GetApplyJobs")]
        public async Task<IActionResult> GetApplyJobs(int PageNumber, int PageSize)
        {
            var res = await _jobService.GetApplyJobs(PageNumber, PageSize);
            if (res == null)
                return BadRequest();
            return Ok(res);
        }



    }
}
