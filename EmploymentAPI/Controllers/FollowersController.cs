using Job.Services.JobServices.IServices;
using Job.Services.JobServices.Results;
using Job.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Job.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FollowersController : ControllerBase
    {
        private readonly IFollowersService _followersService;

        public FollowersController(IFollowersService followersService)
        {
            _followersService = followersService;
        }

        [HttpPost("Follow-Company")]
        public async Task<IActionResult> FollowCompany([FromQuery] string CompanyID)
        {
            var res = await _followersService.FollowCompany(CompanyID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpPost("Un-Follow")]
        public async Task<IActionResult> UnFollow([FromQuery] string CompanyID)
        {
            var res = await _followersService.UnFollow(CompanyID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
    }
}
