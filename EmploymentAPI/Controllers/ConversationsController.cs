using Job.Services.JobServices.DTOs.ConversationsDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Job.API.Controllers
{
    [Authorize]
    [Route("api/Conversations")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        public ConversationsController(IConversationService conversation)
        {
            _conversationService = conversation;
        }

        [HttpPost("Create-Conversation")]
        public async Task<IActionResult> CreateConversation([FromQuery]string UserID)
        {
            var res =await _conversationService.CreateConversation(UserID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res?.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpPost("Send-Message")]
        public async Task<IActionResult> SendMessage([FromBody]MessageDTO message)
        {
            var res = await _conversationService.SendMessage(message);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res?.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpPost("Get-Message")]
        public async Task<IActionResult> GetMessage([FromQuery] DateTime? lastMessageTime, [FromQuery] int PageSize,[FromQuery] int ConversationID)
        {
            var res = await _conversationService.GetMessages(lastMessageTime,PageSize, ConversationID);
            if (!res.Success)
                return BadRequest(ApiResponse<List<GetMessageDTO>>.ErrorResponse(res?.Message));
            return Ok(ApiResponse<List<GetMessageDTO>>.SuccessResponse(res.Data,res.Message));
        }

        [HttpGet("Get-Conversations")]
        public async Task<IActionResult> GetConversations(DateTime? lastMessageTime, int PageSize)
        {
            var res = await _conversationService.GetConversations(lastMessageTime, PageSize);
            if (!res.Success)
                return BadRequest(ApiResponse<List<GetConversationDTO>>.ErrorResponse(res?.Message));
            return Ok(ApiResponse<List<GetConversationDTO>>.SuccessResponse(res.Data,res.Message));
        }

    }
}
