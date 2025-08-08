using Job.Services.JobServices.DTOs.EmployeeDTO;
using Job.Services.JobServices.DTOs.ExperinceDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Job.API.Controllers
{
    [Authorize]
    [Route("api/Experince")]
    [ApiController]
    public class ExperinceController : ControllerBase
    {
        private readonly IExperinceService _experince;
        public ExperinceController(IExperinceService experince )
        {
             _experince= experince;
        }


        [HttpPost("AddExperince")]
        public async Task<IActionResult> AddExperince ([FromBody] ExperinceDTO experienceDTO)
        {
            var res = await _experince.AddExperince (experienceDTO);
            if(!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpPut("UpdateExperince")]
        public async Task<IActionResult> UpdateExperince([FromBody] ExperinceDTO experienceDTO, [FromQuery] int ExperinceID)
        {
            var res =await _experince.UpdateExperince(experienceDTO, ExperinceID);
            if (res == null)
                return BadRequest(ApiResponse.ErrorResponse(res?.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpDelete("DeleteExperince")]

        public async Task<IActionResult> DeleteExperince([FromQuery] int ExperinceID)
        {
            var res =await _experince.DeleteExperince(ExperinceID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
        [HttpPost("ApllyExperince")]
        public async Task<IActionResult> ApllyExperince([FromQuery] int ExperinceID)
        {
            var res =await _experince.ApplyExperince(ExperinceID);
            if (res == null)
                return BadRequest(ApiResponse.ErrorResponse(res?.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }


    }
}
