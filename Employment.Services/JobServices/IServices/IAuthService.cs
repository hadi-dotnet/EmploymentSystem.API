using Job.Services.JobServices.DTOs.AuthDTO;
using Job.Services.JobServices.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.Services
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(RegisterDto dto);

        Task<LoginResult> LoginAsync(LoginDTO dto);

    }

}
