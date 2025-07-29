using Employment.Infrastructure.Entitys;
using Job.Core.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Infrastructure.Context.Configuration
{
    public class Messages
    {
        public int ID { get; set; }
        public int ConversationId { get; set; }
        public string? SenderBy { get; set; }
        public string? MassageText { get; set; }
        public DateTime? SendAT { get; set; }
        public AppUser? AppUser { get; set; }
        public Conversations? Conversation {  get; set; }
    }
}
