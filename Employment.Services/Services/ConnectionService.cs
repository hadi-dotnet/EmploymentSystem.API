using Employment.Infrastructure.Context;
using Employment.Infrastructure.Entitys;
using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.ConnectionsDTO;
using Job.Services.JobServices.IServices;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Esf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Job.Services.Services
{
    public class ConnectionService : IConnectionService
    {
        private readonly AppDBContext _dbContext;
        private readonly IUserService _userservice;

        public ConnectionService(AppDBContext context, IUserService userService)
        {
            _dbContext = context;
            _userservice = userService;
        }

        public async Task<Result> SendConnect(string ReseiverID)
        {
            if (string.IsNullOrWhiteSpace(ReseiverID))
                return Result.Fail("Invaled UserID");

            if (_userservice.GetRole() != UserTypeEnum.Employee.ToString())
                return Result.Fail("You Have No Access");

            var UserID = _userservice.GetCurrentUserID();
            if (UserID == null)
                return Result.Fail("Token Error");

            var data = await (from receiver in _dbContext.Employees
                              where receiver.UserID == ReseiverID
                              from currentUser in _dbContext.Employees
                              where currentUser.UserID == UserID
                              select new
                              {

                                  Receiver = receiver,
                                  CurrentUser = currentUser,
                                  ExistingConnection = _dbContext.Connections
                                      .FirstOrDefault(c => (c.Sender == UserID
                                                        && c.Reseiver == ReseiverID
                                                        && c.Status == EnumConnectStatus.accept) ||
                                                        (c.Sender == UserID
                                                        && c.Reseiver == ReseiverID
                                                        && c.Status == EnumConnectStatus.waiting) ||
                                                        (c.Reseiver == UserID
                                                        && c.Sender == ReseiverID
                                                        && c.Status == EnumConnectStatus.accept) ||
                                                        (c.Reseiver == UserID
                                                        && c.Sender == ReseiverID
                                                        && c.Status == EnumConnectStatus.waiting))
                              })
                              .FirstOrDefaultAsync();

            if (data == null || data.Receiver == null)
                return Result.Fail("User Not Found");

            if (data.CurrentUser == null)
                return Result.Fail("Token Error");

            if (UserID == ReseiverID)
                return Result.Fail("You Cannot Send a Connect To Yourself");

            if (string.IsNullOrWhiteSpace(data.CurrentUser.FirstName)
                || string.IsNullOrWhiteSpace(data.CurrentUser.secoundName)||
                string.IsNullOrWhiteSpace(data.CurrentUser.LastName))
                return Result.Fail("You Have To Enter Your Name");

            if(data.ExistingConnection != null)
            {
                if (data.ExistingConnection.Status == EnumConnectStatus.accept)
                    return Result.Fail("You Are Already Connect");
                else if (data.ExistingConnection.Status == EnumConnectStatus.waiting)
                    return Result.Fail("You Are Already Send the request");
            }

            var SendConnect = new Connections
            {
                Sender = UserID,
                Reseiver = ReseiverID,
                Status = EnumConnectStatus.waiting,
                Created = DateTime.UtcNow
            };

            await _dbContext.Connections.AddAsync(SendConnect);
            await _dbContext.SaveChangesAsync();

            return Result.SuccessResult("Connect Send Success");

        }

        public async Task<Result<List<GetConnectionRequstDTO>>> GetConnectionsRequst()
        {
            if (_userservice.GetRole() != UserTypeEnum.Employee.ToString())
                return Result<List<GetConnectionRequstDTO>>.Fail("You Have No Access");

            var UserID = _userservice.GetCurrentUserID();

            var ConRequstList = await (from con in _dbContext.Connections
                                       join em in _dbContext.Employees
                                       on con.Reseiver equals em.UserID
                                       where con.Reseiver == UserID && con.Status == EnumConnectStatus.waiting
                                       orderby con.Created descending
                                       select new GetConnectionRequstDTO
                                       {
                                           id = con.Id,
                                           UserRequstID = con.Sender,
                                           UserRequstName = con.UserSender.FirstName + " " +
                                           con.UserSender.secoundName
                                           + " " + con.UserSender.LastName

                                       }).ToListAsync();
            
            if(!ConRequstList.Any())
                return Result<List<GetConnectionRequstDTO>>.Fail("Not Found");

            return Result<List<GetConnectionRequstDTO>>.SuccessResult(ConRequstList, "Success");
            
            
                                       
        }

        public async Task<Result> AcceptOrRejectTheConnect(int ConnectID, EnumConnectStatusDTO Status)
        {
            if (_userservice.GetRole() != UserTypeEnum.Employee.ToString())
                return Result.Fail("You Have No access");

            var CurrentUserID = _userservice.GetCurrentUserID();

            var Connect = await _dbContext.Connections.FirstOrDefaultAsync(x => x.Id == ConnectID);
                                                                        

            if (Connect == null)
                return Result.Fail("Connect Not Found");

            if (Connect.Reseiver != CurrentUserID)
                return Result.Fail("You Have No Access");

            if (Connect.Status != EnumConnectStatus.waiting)
                return Result.Fail("You Have Already Made Your Decision");

            if (Status == EnumConnectStatusDTO.cancel || Status == EnumConnectStatusDTO.accept)
                Connect.Status = (EnumConnectStatus)Status;
            else
                return Result.Fail("Invaled Status");


            await _dbContext.SaveChangesAsync();

            if (Status == EnumConnectStatusDTO.accept)
                return Result.SuccessResult("Accept Success");
            else
                return Result.SuccessResult("Cancel Success");

        }

        public async Task<Result> UnConnect(int ConnectID)
        {
            if (_userservice.GetRole() != UserTypeEnum.Employee.ToString())
                return Result.Fail("You Have No access");

            if (ConnectID < 0)
                return Result.Fail("Invaled Input");

            var UserID = _userservice.GetCurrentUserID();

            var Connect = await _dbContext.Connections.FirstOrDefaultAsync(x=>x.Id == ConnectID &&
                                                            x.Status == EnumConnectStatus.accept );
                                                           
            if (Connect == null)
                return Result.Fail("Connect Not Found");

            if(Connect.Reseiver == UserID || Connect.Sender == UserID)
            {
                Connect.Status = EnumConnectStatus.cancel;
                await _dbContext.SaveChangesAsync();

                return Result.SuccessResult("UnConnect Success");
            }

            return Result.Fail("You Have No Access");

        }

        public async Task<Result<PageResult<GetConnectionsDTO>>> GetConnections(int PageNumber,int PageSize)
        {
            if (_userservice.GetRole() != UserTypeEnum.Employee.ToString())
                return Result<PageResult<GetConnectionsDTO>>.Fail("You Have No Access");

            if (PageNumber<0 || PageSize < 0)
                return Result<PageResult<GetConnectionsDTO>>.Fail("Invaled Input");

            var UserID = _userservice.GetCurrentUserID();
            if (UserID == null)
                return Result<PageResult<GetConnectionsDTO>>.Fail("Token Error");

            var connectionsList = await _dbContext.Connections.Where(x => (x.Sender == UserID || x.Reseiver == UserID) &&
                                        x.Status == EnumConnectStatus.accept).Skip((PageNumber - 1) * PageSize).Take(PageSize)
                                        .Select(x => new GetConnectionsDTO
                                        {
                                            id = x.Id,
                                            FullName = x.Sender == UserID ? $"{x.UserReseiver.FirstName} {x.UserReseiver.secoundName} {x.UserReseiver.LastName}"
                                            : $"{x.UserSender.FirstName} {x.UserSender.secoundName} {x.UserSender.LastName}"
                                        }).ToListAsync();

            if(!connectionsList.Any())
                return Result<PageResult<GetConnectionsDTO>>.Fail("Not Found");

            return Result<PageResult<GetConnectionsDTO>>.SuccessResult(new PageResult<GetConnectionsDTO>
            {
                PageNumber = PageNumber,
                PageSize=PageSize,
                Items = connectionsList
            },"Success");



        }

    }
}
