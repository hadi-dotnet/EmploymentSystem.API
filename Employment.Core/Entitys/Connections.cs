using Employment.Infrastructure.Entitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job.Core.Entitys
{
    public enum EnumConnectStatus
    {
        accept = 1, cancel = 0,waiting = 2
    }

    public class Connections
    {
        public int Id { get; set; } 
        public string? Sender { get; set; }
        public string? Reseiver { get; set; }
        public DateTime? Created { get; set; }
        public EnumConnectStatus Status { get; set; }
        public Employees UserSender { get; set; } 
        public Employees UserReseiver{ get; set; } 
    }
}
