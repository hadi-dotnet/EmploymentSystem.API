using Employment.Infrastructure.Entitys;
using Job.Services.Business;
using Job.Services.JobServices.DTOs.ComapnyDTO;
using Job.Services.JobServices.DTOs.EmployeeDTO;
using Job.Services.JobServices.DTOs.SkillsDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace Job.API.Controllers
{
    [Authorize]
    [Route("api/Employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employee;
        private readonly ICompanyService _company;

        public EmployeeController(IEmployeeService employee,ICompanyService company)
        {
            _employee = employee;
            _company = company;
        } 

        [HttpPost("UpdateEmployee")]
        
        public async Task< IActionResult> UpdateEmployee([FromBody] EmployeeDTO updateEmployee)
        {           
            var res = await _employee.UpdateEmployee(updateEmployee);
            if(!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));          
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }


        [HttpGet("GetInformation")]
        public async Task<IActionResult> GetInformation()
        {
            var Role = User.FindFirstValue(ClaimTypes.Role);
            if (Role == UserTypeEnum.Employee.ToString())
            {
                var res =await _employee.GetEmployeeInformation();
                if (res == null)
                    return BadRequest(ApiResponse<GetInforamtionDTO>.ErrorResponse(res?.Message));
                return Ok(ApiResponse<GetInforamtionDTO>.SuccessResponse(res.Data,res.Message));
            }
            else
            {
                var res = await _company.GetCompanyInformation();
                if (!res.Success)
                    return BadRequest(ApiResponse<CompanyDTO>.ErrorResponse(res.Message));
                return Ok(ApiResponse<CompanyDTO>.SuccessResponse(res.Data,res.Message));
            }        
        }

        [HttpGet("FindSkill")]
        public async Task<IActionResult> FindSkills ([FromQuery]string SkillName)
        {
            var res = await _employee.FindSkills(SkillName);
            if (!res.Success)
                return BadRequest(ApiResponse<List<FindSkillsDTO>>.ErrorResponse(res?.Message));
            return Ok(ApiResponse<List<FindSkillsDTO>>.SuccessResponse(res.Data,res.Message));
        }

        [HttpPost("AddSkills")]
        public async Task<IActionResult> AddSkills ([FromBody]List<AddSkillDTO> skills)
        {
            var res = await _employee.AddSkill(skills);
            if(!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [AllowAnonymous]
        [HttpPost("ApplySkill")]
        public async Task<IActionResult> ApplySkill([FromQuery]int SkillID)
        {           
            var res =  await _employee.Applyskill(SkillID);
            if(!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
        [HttpPut("UpdateSkill")]
        public async Task<IActionResult> UpdateSkill ([FromQuery] int SkillID,[FromQuery] string skillName)
        {         
            var res = await _employee.UpdateSkill(SkillID,skillName);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpDelete("DeleteSkill")]
        public async Task<IActionResult> DeleteSkill([FromQuery] int SkillID)
        {
            var res = await _employee.DeleteSkill(SkillID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }


    }
}
