using Job.Core.Entitys;
using Job.Services.JobServices.DTOs.ConnectionsDTO;
using Job.Services.JobServices.IServices;
using Job.Services.JobServices.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Job.API.Controllers
{
    [Authorize]
    [Route("api/Connections")]
    [ApiController]
    public class ConnectionsController : ControllerBase
    {
        private readonly IConnectionService _connectservice;
        public ConnectionsController(IConnectionService connectionService)
        {
            _connectservice = connectionService;
        }

        [HttpPost("Send-Connect")]
        public async Task<IActionResult> SendConnect ([FromQuery] string ReseiverID)
        {
            var res = await _connectservice.SendConnect(ReseiverID);
            if(!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpGet("Get-Connections-Requst")]
        public async Task<IActionResult> GetConnectionsRequst()
        {
            var res = await _connectservice.GetConnectionsRequst();
            if (!res.Success)
                return BadRequest(ApiResponse<List< GetConnectionRequstDTO>>.ErrorResponse(res.Message));
            return Ok(ApiResponse<List<GetConnectionRequstDTO>>.SuccessResponse(res.Data, res.Message));
        }

        [HttpPost("Accept-Or-Reject-The-Connect")]
        public async Task<IActionResult> AcceptOrRejectTheConnect([FromQuery] int ConnectID , [FromQuery] EnumConnectStatusDTO Status)
        {
            var res = await _connectservice.AcceptOrRejectTheConnect(ConnectID , Status);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpPost("Un-Connect")]
        public async Task<IActionResult> UnConnect ([FromQuery] int ConnectID )
        {
            var res = await _connectservice.UnConnect(ConnectID);
            if(!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpGet("Get-Connections")]
        public async Task<IActionResult> GetConnections([FromQuery] int PageNumber, [FromQuery]int PageSize)
        {
            var res = await _connectservice.GetConnections(PageNumber,PageSize);
            if (!res.Success)
                return BadRequest(ApiResponse<PageResult<GetConnectionsDTO>>.ErrorResponse(res.Message));
            return Ok(ApiResponse<PageResult<GetConnectionsDTO>>.SuccessResponse(res.Data,res.Message));
        }
    }
}
