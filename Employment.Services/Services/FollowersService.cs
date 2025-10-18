using Employment.Infrastructure.Context;
using Job.Core.Entitys;
using Job.Services.JobServices.IServices;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.Services
{
    public class FollowersService: IFollowersService
    {
        private readonly AppDBContext _dBContext;
        private readonly IUserService _userService;

        public FollowersService(AppDBContext dBContext, IUserService userService)
        {
            _dBContext = dBContext;
            _userService = userService;
        }

        public async Task<Result> FollowCompany(string CompanyID)
        {
            if (string.IsNullOrWhiteSpace(CompanyID))
                return Result.Fail("Invaled Input");

            var UserID = _userService.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Token Error");

            if (UserID == CompanyID)
                return Result.Fail("You Cant Follow Yourself");

            var companyExists = await _dBContext.Companies.AnyAsync(x => x.UserID == CompanyID);
            if (!companyExists)
                return Result.Fail("Not Found");

            var alreadyFollowed = await _dBContext.Followers.AnyAsync(x => x.CompanyID == CompanyID && x.FollowerID == UserID);               
            if (alreadyFollowed)
                return Result.Fail("You Already Follow This Company");

            var Follow = new Followers
            {
                CompanyID = CompanyID,
                FollowerID = UserID
            };

            await _dBContext.Followers.AddAsync(Follow);
            await _dBContext.SaveChangesAsync();

            return Result.SuccessResult("Followed");

        }

        public async Task<Result> UnFollow(string CompanyID)
        {
            if (string.IsNullOrWhiteSpace(CompanyID))
                return Result.Fail("Invaled Input");

            var UserID = _userService.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Tokent Error");

            var Follow = await _dBContext.Followers.FirstOrDefaultAsync(x=>x.CompanyID == CompanyID&&x.FollowerID == UserID);
            if (Follow==null)
                return Result.Fail("You Already UnFollow This Company");

            _dBContext.Followers.Remove(Follow);
            await _dBContext.SaveChangesAsync();
            return Result.SuccessResult("UnFollowed");
        }
    }
}
