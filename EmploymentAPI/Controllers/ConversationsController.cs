using Job.Services.JobServices.DTOs.ConversationsDTO;
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
        public async Task<IActionResult> CreateConversation(string UserID)
        {
            var res =await _conversationService.CreateConversation(UserID);
            if (res == null)
                return BadRequest(res.Message);
            return Ok(res.Message);
        }

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(MessageDTO message)
        {
            var res = await _conversationService.SendMessage(message);
            if (res == null)
                return BadRequest(res.Message);
            return Ok(res.Message);
        }

        [HttpPost("GetMessage")]
        public async Task<IActionResult> GetMessage(int ConvID)
        {
            var res = await _conversationService.GetMessages(ConvID);
            if (res == null)
                return BadRequest();
            return Ok(res);
        }

        [HttpGet("GetConversations")]
        public async Task<IActionResult> GetConversations()
        {
            var res = await _conversationService.GetConversations();
            if (res == null)
                return BadRequest();
            return Ok(res);
        }

    }
}
