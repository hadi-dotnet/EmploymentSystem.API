using Job.API.Dtos;
using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.ComapnyDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Job.API.Controllers
{
    [Authorize]
    [Route("api/Company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _company;
        public CompanyController(ICompanyService Icom)
        {
            _company = Icom;    
        }

        [HttpPut("Update-Company")]
        public async Task<IActionResult> UpdateCompany([FromForm] UpdateCompanyDTO companyDTOApi)
        {        
            var res = await _company.UpdateCompany(companyDTOApi);
            if(!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));          
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
        
        [HttpGet("Find-Company")]
        public async Task<IActionResult> FindCompany([FromQuery] string companyName)
        {
            var res = await _company.FindCompany(companyName);
            if (!res.Success)
                return NotFound(ApiResponse<List<FindCompanyDTO>>.ErrorResponse(res?.Message));
            return Ok(ApiResponse<List<FindCompanyDTO>>.SuccessResponse(res.Data,res.Message));
        }


    }
}
