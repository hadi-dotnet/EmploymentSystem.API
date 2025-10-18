using Job.Services.JobServices.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.IServices
{
    public interface IFollowersService
    {
        Task<Result> FollowCompany(string CompanyID);
        Task<Result> UnFollow(string CompanyID);
    }
}
