using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.ConnectionsDTO;
using Job.Services.JobServices.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.IServices
{
    public interface IConnectionService
    {
        Task<Result> SendConnect(string ReseiverID);
        Task<Result<List<GetConnectionRequstDTO>>> GetConnectionsRequst();
        Task<Result> AcceptOrRejectTheConnect(int ConnectID, EnumConnectStatusDTO Status);
        Task<Result> UnConnect(int ConnectID);
        Task<Result<PageResult<GetConnectionsDTO>>> GetConnections(int PageNumber, int PageSize);
    }
}
