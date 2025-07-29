using Employment.Infrastructure.Entitys;
using Job.Services.Business;
using Job.Services.JobServices.DTOs.EmployeeDTO;
using Job.Services.JobServices.DTOs.SkillsDTO;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var result = await _employee.UpdateEmployee(updateEmployee);
            if(!result.Success)
                return BadRequest(result.Message);          
            return Ok(result.Message);
        }


        [HttpGet("GetInformation")]
        public async Task<IActionResult> GetInformation()
        {
            var Role = User.FindFirstValue(ClaimTypes.Role);
            if (Role == UserTypeEnum.Employee.ToString())
            {
                var res =await _employee.GetEmployeeInformation();
                if (res == null)
                    return BadRequest();
                return Ok(res);
            }
            else
            {
                var res = await _company.GetCompanyInformation();
                if (res == null)
                    return BadRequest();
                return Ok(res);
            }        
        }

        [HttpGet("FindSkill")]
        public async Task<ActionResult<List<FindSkillsDTO>>> FindSkills ([FromQuery]string SkillName)
        {
            var SkillList = await _employee.FindSkills(SkillName);
            if (SkillList == null)
                return NotFound();
            return Ok(SkillList);
        }

        [HttpPost("AddSkills")]
        public async Task<IActionResult> AddSkills ([FromBody]List<AddSkillDTO> skills)
        {
            var res = await _employee.AddSkill(skills);
            if(!res.Success)
                return BadRequest(res.Message);
            return Ok(res.Message);
        }

        [AllowAnonymous]
        [HttpPost("ApplySkill")]
        public async Task<IActionResult> ApplySkill([FromQuery]int SkillID)
        {           
            var result =  await _employee.Applyskill(SkillID);
            if(!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
        [HttpPut("UpdateSkill")]
        public async Task<IActionResult> UpdateSkill ([FromQuery] int SkillID,[FromQuery] string skillName)
        {         
            var result = await _employee.UpdateSkill(SkillID,skillName);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("DeleteSkill")]
        public async Task<IActionResult> DeleteSkill([FromQuery] int SkillID)
        {
            var result = await _employee.DeleteSkill(SkillID);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }


    }
}
