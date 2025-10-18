using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job.Services.JobServices.Services;

namespace Job.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetCurrentUserID()
        {
            return _contextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public string GetRole()
        {
            return _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        }


    }
}
