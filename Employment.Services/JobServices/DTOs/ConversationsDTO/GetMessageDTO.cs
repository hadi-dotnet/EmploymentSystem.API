using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Services.JobServices.DTOs.ConversationsDTO
{
    public class GetMessageDTO
    { 
        public int ID { get; set; } 
        public string? Message { get; set; }
        public DateTime? SendAT { get; set; }
    }
}
