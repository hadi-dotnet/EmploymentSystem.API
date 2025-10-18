using Job.Services.JobServices.DTOs.AuthDTO;
using Job.Services.JobServices.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.Services
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(RegisterDto dto);
        Task<LoginResult> LoginAsync(LoginDTO dto);
        Task<Result> ConfirmEmailAsync(string userId, string tokenEncoded);
        Task<Result> ResendConfirmationAsync(EmailDTO email);
        Task<Result> FrorgetPassword(string Email);
        Task<Result> ResetPassword(ResetPasswordDTO reset);
        Task<Result> ResetPasswordInsideSystem(ResetPasswordInsideSystemDTO resetDTO);
        Task<Result> ChangeEmail(EmailDTO Email);
    }

}
