using Employment.Infrastructure.Entitys;
using Job.Infrastructure.Context.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public class Conversations
    {
        public  int ID { get; set; }    
        public string? UserID1 { get; set; }
        public string? UserID2 { get; set; }
        public DateTime? CreatedAT { get; set; }
        public AppUser? AppUser1 { get; set; } 
        public AppUser? AppUser2 { get; set; }
        public ICollection<Messages> Message { get; set; } = new List<Messages>();
    }
}
