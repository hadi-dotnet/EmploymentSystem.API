using Job.Services.JobServices.DTOs.ConversationsDTO;
using Job.Services.JobServices.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.Services
{
    public interface IConversationService
    {
        Task<Result> CreateConversation(string UserID2);
        Task<Result> SendMessage(MessageDTO message);
        Task<List<GetMessageDTO>?> GetMessages(int ConversationID);
        Task<List<GetConversationDTO>> GetConversations();
       
    }
}
