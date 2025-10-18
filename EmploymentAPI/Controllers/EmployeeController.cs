using Employment.Infrastructure.Entitys;
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
        private readonly IUserService _userservice;

        public EmployeeController(IEmployeeService employee,ICompanyService company ,IUserService userService)
        {
            _employee = employee;
            _company = company;
            _userservice = userService;
        } 

        [HttpPost("Update-Employee")]
        
        public async Task< IActionResult> UpdateEmployee([FromForm] EmployeeDTO updateEmployee)
        {           
            var res = await _employee.UpdateEmployee(updateEmployee);
            if(!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));          
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpGet("Get-Employee-Information-By-ID")]

        public async Task<IActionResult> GetEmployeeInformationByID([FromQuery] string EmployeeID)
        {
            var res = await _employee.GetEmployeeInformationByID(EmployeeID);
            if (!res.Success)
                return BadRequest(ApiResponse<GetInforamtionDTO>.ErrorResponse(res.Message));
            return Ok(ApiResponse<GetInforamtionDTO>.SuccessResponse(res.Data,res.Message));
        }

        [HttpGet("Get-Information-For-Employee-Or-Company")]
        public async Task<IActionResult> GetInformation()
        {
            var Role = _userservice.GetRole();
            if (Role == UserTypeEnum.Employee.ToString())
            {
                var res =await _employee.GetEmployeeInformation();
                if (!res.Success)
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

        [HttpGet("Find-Skill")]
        public async Task<IActionResult> FindSkills ([FromQuery]string SkillName)
        {
            var res = await _employee.FindSkills(SkillName);
            if (!res.Success)
                return BadRequest(ApiResponse<List<FindSkillsDTO>>.ErrorResponse(res?.Message));
            return Ok(ApiResponse<List<FindSkillsDTO>>.SuccessResponse(res.Data,res.Message));
        }

        [HttpPost("Add-Skills")]
        public async Task<IActionResult> AddSkills ([FromBody] List<AddSkillDTO> skills)
        {
            var res = await _employee.AddSkill(skills);
            if(!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
       
        [HttpPost("Apply-Skill")]
        public async Task<IActionResult> ApplySkill([FromQuery]int SkillID)
        {           
            var res =  await _employee.Applyskill(SkillID);
            if(!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
    
        [HttpDelete("Delete-Skill")]
        public async Task<IActionResult> DeleteSkill([FromQuery] int SkillID)
        {
            var res = await _employee.DeleteSkill(SkillID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }


    }
}
