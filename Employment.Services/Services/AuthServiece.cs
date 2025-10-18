using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.Core.Entitys;
using Job.Core.Helper;
using Job.Infrastructure.settings;
using Job.Services.JobServices.DTOs.AuthDTO;
using Job.Services.JobServices.IServices;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Job.Services.Services
{
    public class AuthServiece : IAuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDBContext _context;
        private readonly IEmailService _emailSender;  
        private readonly IConfiguration _config;       
        private readonly IUserService _userService;       

        public AuthServiece(
            UserManager<AppUser> userManager,
            AppDBContext context,
            IOptions<JwtSettings> jwtSettings,
            IEmailService emailSender,               
            IConfiguration config,
            IUserService userService)                   
        {
            _userManager = userManager;
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _emailSender = emailSender;
            _config = config;
            _userService = userService;
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
       
        private struct LinkAndToken
        {
            public string link, token;
        }

        private async Task<LinkAndToken> GenerateEmailConfirmationLinkAsync(AppUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = TokenHelper.Encode(token); 
            var st = new LinkAndToken();

            var clientUrl = _config["AppSettings:ClientUrl"] ?? "http://localhost:5148";

           
             st.link = $"{clientUrl}/auth/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(encodedToken)}";
            st.token = Uri.EscapeDataString(encodedToken);
            return st;
        }

        private async Task<LinkAndToken> GenerateResetPasswordLinkAsync(AppUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = TokenHelper.Encode(token);
            var st = new LinkAndToken();

            var clientUrl = _config["AppSettings:ClientUrl"] ?? "http://localhost:5148";

       
            st.link = $"{clientUrl}/auth/reset-password?userId={user.Id}&token={Uri.EscapeDataString(encodedToken)}";
            st.token = Uri.EscapeDataString(encodedToken);
            return st;
        }

        public async Task<Result> RegisterAsync(RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.UserName))
                return Result.Fail("Invalid Email");

            if (!Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Result.Fail("Invaled Email");

            if (Convert.ToInt32(dto.UserType) != 1 && Convert.ToInt32(dto.UserType) != 2)
                return Result.Fail("Invaled UserType");

            if(await _userManager.FindByEmailAsync(dto.Email) != null)
                return Result.Fail("Email Exest");


            var user = new AppUser
            {
                UserName = dto.UserName.ToLower(),
                Email = dto.Email.ToLower(),
                UserType = dto.UserType
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return Result.Fail(errors);
            }

            if (dto.UserType == UserTypeEnum.Employee)
            {
                var employee = new Employees { UserID = user.Id };
                _context.Employees.Add(employee);
            }
            else 
            {
                var company = new Company { UserID = user.Id };
                _context.Companies.Add(company);
            }           

            await _context.SaveChangesAsync();

            var confirmLink = await GenerateEmailConfirmationLinkAsync(user);

            var html = $@"
            <p>مرحبا {dto.UserName},</p>
            <p>اضغط الرابط لتأكيد الإيميل:</p>
            <p><a href=""{confirmLink.link}"">تأكيد الإيميل</a></p>
            <p>وهنا المعلومات جاهزة<p>
            <p>UserID = {user.Id}<p>
            <p>Token = {confirmLink.token}<p>";

            try
            {
                await _emailSender.SendEmailAsync(user.Email, "Employment System - تأكيد الايميل", html);
            }
            catch (Exception ex)
            {
                return Result.Fail($"User registered but sending confirmation email failed: {ex.Message}");
            }

            return Result.SuccessResult("User registered successfully! Please check your email to confirm your account.");
            
        }

        public async Task<LoginResult> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserNameOrEmail)
                       ?? await _userManager.FindByEmailAsync(dto.UserNameOrEmail);

            if (user == null)
                return new LoginResult { Success = false, Message = "Invalid username or password." };
            
           
            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
            {
                return new LoginResult { Success = false, Message = "Invalid username or password." };
            }

            var token = GenerateJwtToken(user);

            return new LoginResult
            {
                Success = true,
                Token = token,
                Message = "Login successful!"
            };
        }

        
        public async Task<Result> ConfirmEmailAsync(string userId, string tokenEncoded)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(tokenEncoded))
                return Result.Fail("Invaled Input");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Fail("User not found.");

            if(await _userManager.IsEmailConfirmedAsync(user))
                return Result.Fail("Email already confirmed.");

            try
            {
               
                var urlDecoded = Uri.UnescapeDataString(tokenEncoded);
                var decodedToken = TokenHelper.SafeDecode(urlDecoded);

                var res = await _userManager.ConfirmEmailAsync(user, decodedToken);
                if (!res.Succeeded)
                {
                    var errors = string.Join("; ", res.Errors.Select(e => e.Description));
                    return Result.Fail(errors);
                }

                return Result.SuccessResult("Email confirmed successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Invalid token or decoding error: {ex.Message}");
            }
        }

       
        public async Task<Result> ResendConfirmationAsync(EmailDTO email)
        {        
            if(!Regex.IsMatch(email.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Result.Fail("Invaled Email");

            var user = await _userManager.FindByEmailAsync(email.Email);
            if (user == null)
                return Result.Fail("User not found.");

            if (await _userManager.IsEmailConfirmedAsync(user))
                return Result.Fail("Email already confirmed.");
             
            var confirmLink = await GenerateEmailConfirmationLinkAsync(user);
            var html = $@"
                        <p>مرحبا {user.UserName},</p>
                        <p>اضغط الرابط لتأكيد الإيميل:</p>
                        <p><a href=""{confirmLink.link}"">تأكيد الإيميل</a></p>
                        <p>وهنا المعلومات جاهزة<p>
                        <p>UserID = {user.Id}<p>
                        <p>Token = {confirmLink.token}<p>"";";

            try
            {
                await _emailSender.SendEmailAsync(user.Email, "Employment System - تأكيد الإيميل", html);
                return Result.SuccessResult("Confirmation email sent.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Sending email failed: {ex.Message}");
            }
        }

        public async Task<Result> FrorgetPassword(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
                return Result.Fail("Invaled Input");

            if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Result.Fail("Invaled Email");

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
                return Result.Fail("User Not Found");

            var ResetPassLink = await GenerateResetPasswordLinkAsync(user);
            var html = $@"
                         <p>مرحبا {user.UserName},</p>
                         <p>اضغط الرابط لتغيير كلمة السر:</p>
                         <p><a href=""{ResetPassLink.link}"">تغيير كلمة السر</a></p>
                         <p>وهنا المعلومات جاهزة<p>
                         <p>UserID = {user.Id}<p>
                         <p>Token = {ResetPassLink.token}<p>"";";


            try
            {
                 await _emailSender.SendEmailAsync(Email, "تغيير كلمة السر - Employment System", html);
                return Result.SuccessResult("reset password sent.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Sending email failed: {ex.Message}");
               
            }

           
        }

        public async Task<Result> ResetPassword(ResetPasswordDTO resetPass )
        {
            if (string.IsNullOrWhiteSpace(resetPass.UserID) || string.IsNullOrWhiteSpace(resetPass.token) || string.IsNullOrWhiteSpace(resetPass.NewPassword))
                return Result.Fail("Invaled Input");
            var user  = await _userManager.FindByIdAsync(resetPass.UserID);
            if (user == null)
                return Result.Fail("User Not Found");

            try
            {
                var urlDecoded = Uri.UnescapeDataString(resetPass.token);
                var decodedToken = TokenHelper.SafeDecode(urlDecoded);
                var res = await _userManager.ResetPasswordAsync(user, decodedToken, resetPass.NewPassword);
                if(!res.Succeeded)
                {
                    var errors = string.Join("; ", res.Errors.Select(e => e.Description));
                    return Result.Fail(errors);
                }
                return Result.SuccessResult("Reset Password Success");

            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
              
            }
        }

        public async Task<Result> ResetPasswordInsideSystem(ResetPasswordInsideSystemDTO resetDTO)
        {
            if (string.IsNullOrWhiteSpace(resetDTO.OldPassword) || string.IsNullOrWhiteSpace(resetDTO.NewPassword))
                return Result.Fail("Invaled Input");

            if (resetDTO.OldPassword == resetDTO.NewPassword)
                return Result.Fail("Old password same as new");

            if (resetDTO.NewPassword != resetDTO.ConfirmPassword)
                return Result.Fail("Password Not Match");

            var UserID = _userService.GetCurrentUserID();
            if (UserID == null) 
                return Result.Fail("Token Error");

            var user = await _userManager.FindByIdAsync(UserID);
            if (user == null)
                return Result.Fail("User Not Found");

            var SetNewPass = await _userManager.ChangePasswordAsync(user, resetDTO.OldPassword, resetDTO.NewPassword);
            if (!SetNewPass.Succeeded)
            {
                var error = string.Join(";",SetNewPass.Errors.Select(e => e.Description));
                return Result.Fail(error);
            }

            return Result.SuccessResult("Change Password Success");
        }

        public async Task<Result> ChangeEmail(EmailDTO Email)
        {
            if (!Regex.IsMatch(Email.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return Result.Fail("Invaled Email");

            var UserID = _userService.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Token Error");

            var user = await _userManager.FindByIdAsync(UserID);
            if (user == null)
                return Result.Fail("User not found.");

            var result = await _userManager.SetEmailAsync(user, Email.Email);
            if (!result.Succeeded)
                return Result.Fail("Faild");

            return Result.SuccessResult("Success");

        }
    }
}
