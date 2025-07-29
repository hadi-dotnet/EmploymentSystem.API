using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.Core.Entitys;
using Job.Infrastructure.settings;
using Job.Services.JobServices.DTOs.AuthDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Job.Services.Business
{
    public class AuthServiece : IAuthService
    {
        private readonly JwtSettings _jwtSettings;
         private readonly UserManager<AppUser> _userManager;
        private readonly AppDBContext _context;

        public AuthServiece(UserManager<AppUser> userManager, AppDBContext context, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        private string GenerateJwtToken(AppUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(ClaimTypes.Role, user.UserType.ToString()), 
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)); 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(48),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Result> RegisterAsync(RegisterDto dto)
        {
            var user = new AppUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                UserType =  dto.UserType
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return new Result
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            if (dto.UserType == UserTypeEnum.Employee) 
            {
                var employee = new Employees { UserID = user.Id };
                _context.Employees.Add(employee);
            }
            else if (dto.UserType == UserTypeEnum.Company)
            {
                var company = new  Company { UserID = user.Id };
                _context.Companies.Add(company);
            }
            else
            {
                return new Result
                {
                    Success = false,
                    Errors = new List<string> { "Invalid UserType" },
                    Message = "Faild"
                };
            }

            await _context.SaveChangesAsync();

            return new Result
            {
                Success = true,
                Message = "User registered successfully!"
            };
        }


        public async Task<LoginResult> LoginAsync(LoginDTO dto)
        {          
            var user = await _userManager.FindByNameAsync(dto.UserNameOrEmail)
                       ?? await _userManager.FindByEmailAsync(dto.UserNameOrEmail);

            if (user == null)
            {
                return new LoginResult { Success = false, Errors = new List<string> { "Invalid username or password." } };
            }
          
            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
            {
                return new LoginResult { Success = false, Errors = new List<string> { "Invalid username or password." } };
            }
          
            var token = GenerateJwtToken(user);

            return new LoginResult
            {
                Success = true,
                Token = token,
                Message = "Login successful!"
            };
        }

    }
}
