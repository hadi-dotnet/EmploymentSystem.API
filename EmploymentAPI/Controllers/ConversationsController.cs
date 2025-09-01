using Job.Services.JobServices.DTOs.ConversationsDTO;
using Job.Services.JobServices.Results;
using Job.Services.JobServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Job.API.Controllers
{
    [Route("api/Conversations")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        public ConversationsController(IConversationService conversation)
        {
            _conversationService = conversation;
        }

        [HttpPost("CreateConversation")]
        public async Task<IActionResult> CreateConversation([FromQuery]string UserID)
        {
            var res =await _conversationService.CreateConversation(UserID);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res?.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody]MessageDTO message)
        {
            var res = await _conversationService.SendMessage(message);
            if (!res.Success)
                return BadRequest(ApiResponse.ErrorResponse(res?.Message));
            return Ok(ApiResponse.SuccessResponse(res.Message));
        }

        [HttpPost("GetMessage")]
        public async Task<IActionResult> GetMessage([FromQuery] int ConvID)
        {
            var res = await _conversationService.GetMessages(ConvID);
            if (!res.Success)
                return BadRequest(ApiResponse<List<GetMessageDTO>>.ErrorResponse(res?.Message));
            return Ok(ApiResponse<List<GetMessageDTO>>.SuccessResponse(res.Data,res.Message));
        }

        [HttpGet("GetConversations")]
        public async Task<IActionResult> GetConversations()
        {
            var res = await _conversationService.GetConversations();
            if (!res.Success)
                return BadRequest(ApiResponse<List<GetConversationDTO>>.ErrorResponse(res?.Message));
            return Ok(ApiResponse<List<GetConversationDTO>>.SuccessResponse(res.Data,res.Message));
        }

    }
}
