using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Job.Services.JobServices.Services
{
    public interface IUserService
    {
        public string GetCurrentUserID();
        public string GetRole();
    }
}
