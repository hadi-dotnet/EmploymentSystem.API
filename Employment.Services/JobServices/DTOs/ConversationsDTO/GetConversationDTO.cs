using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.ConversationsDTO
{
    public class GetConversationDTO
    {
        public int ConversationID { get; set; }
        public string? ReceiverName { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public string? LastMessage { get; set; }

    }
}
