using Job.Core.Entitys;
using Job.Services.Business;
using Job.Services.JobServices.DTOs.ComapnyDTO;
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
        private readonly IManageImageService _ManageImage;
        private readonly IUserService _userservice;
        public CompanyController(ICompanyService Icom, IManageImageService Manageimage,IUserService userService)
        {
            _company = Icom;    
            _ManageImage = Manageimage;
            _userservice = userService;
        }

        [HttpPut("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany([FromForm] Dtos.CompanyDTOAPI companyDTOApi)
        {

            var Userrole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (Userrole != "Company")
            {
                return Unauthorized();
            }


            var company = new CompanyDTO();
            company.ImagePath =await _company.SetImage(companyDTOApi.Logo);
            company.Address = companyDTOApi.Address;
            company.About = companyDTOApi.About;
            company.Name = companyDTOApi.Name;



            
            var Result = await _company.UpdateCompany(company);
            if(!Result.Success)
            {
                return BadRequest(Result.Message);
            }

            return Ok(Result.Message);
                

                
            


        }

        //[HttpGet("GetCompanyInformation")]

        //public async Task<ActionResult<CompanyDTO>> GetCompanyInformation()
        //{
        //    var UserRole = User.FindFirst(ClaimTypes.Role)?.Value;
        //    if (UserRole != "Company")  return Unauthorized();

            

        // var CompanyDTO = await _company.GetCompanyInformation();
        //    if (CompanyDTO == null) return NotFound("Company IS NOT Found");

        //    return Ok(CompanyDTO);
            
        //}
        [AllowAnonymous]
        [HttpGet("FindCompany")]
        public async Task<ActionResult<List< FindCompanyDTO>>> FindCompany(string companyName)
        {
            var CompanyList = await _company.FindCompany(companyName);
            if (CompanyList == null) return NotFound("Not Found");

            return Ok(CompanyList);


        }


    }
}
