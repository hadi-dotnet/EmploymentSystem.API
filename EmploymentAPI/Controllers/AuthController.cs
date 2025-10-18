using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.AuthDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace EmploymentAPI.Controllers
{
 
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
         private readonly IAuthService _authService;

         public AuthController(IAuthService authService)
         {
             _authService = authService;
         }

         [HttpPost("register")]
         public async Task<IActionResult> Register([FromBody] RegisterDto dto)
         {
              var result = await _authService.RegisterAsync(dto);
              if (!result.Success)
                  return BadRequest(ApiResponse.ErrorResponse(result.Message));
              return Ok(ApiResponse.SuccessResponse(result.Message));
          }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (!result.Success)
            {          
                return BadRequest(ApiResponse.ErrorResponse(result.Message));
            }
            return Ok((new { message = result.Message, token = result.Token }));
        }

         [HttpPost("confirm-email")]
         public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
           {
               var res = await _authService.ConfirmEmailAsync(userId, token);
               if (!res.Success)
                   return BadRequest(ApiResponse.ErrorResponse(res.Message));
               return Ok(ApiResponse.SuccessResponse(res.Message));
           }

        [HttpPost("Send-Email-confirmation")]
        public async Task<IActionResult> SendEmailconfirmation([FromBody] EmailDTO emailDTO)
        {
              var res = await _authService.ResendConfirmationAsync(emailDTO);
              if (!res.Success)
                   return BadRequest(ApiResponse.ErrorResponse(res.Message));
              return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpPost("Forget-Password")]
        public async Task<IActionResult> ForgetPassword(EmailDTO emailDTO)
        {
            var res = await _authService.FrorgetPassword(emailDTO.Email);
            if (!res.Success) 
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }
        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO reset)
        {
            var res = await _authService.ResetPassword(reset);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [Authorize]
        [HttpPost("Reset-Password-Inside-System")]
        public async Task<IActionResult> ResetPasswordInsideSystem(ResetPasswordInsideSystemDTO reset)
        {
            var res = await _authService.ResetPasswordInsideSystem(reset);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [Authorize]
        [HttpPost("Change-Email")]
        public async Task<IActionResult> ChangeEmail(EmailDTO Email)
        {
            var res = await _authService.ChangeEmail(Email);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

    }
}
